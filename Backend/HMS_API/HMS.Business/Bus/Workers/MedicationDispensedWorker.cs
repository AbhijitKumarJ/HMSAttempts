using HMS.Bus;
using HMS.Data.DBModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace HMS.Business.Bus.Workers;

public record MedicationDispensedEvent(string Sku, int Quantity, int PatientId, string Reason);

public class MedicationDispensedWorker : EventConsumerBackgroundService<MedicationDispensedEvent>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<MedicationDispensedWorker> _logger;

    public MedicationDispensedWorker(
        IEventBus eventBus,
        IServiceScopeFactory scopeFactory,
        ILogger<MedicationDispensedWorker> logger)
        : base(eventBus, scopeFactory, logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        EventType = "Medication.Dispensed";
    }

    protected override async Task ProcessEventAsync(Guid eventId, MedicationDispensedEvent payload, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HMSContext>();

        var patient = await context.PatPatients
            .FirstOrDefaultAsync(p => p.Id == payload.PatientId, cancellationToken);

        if (patient == null)
        {
            _logger.LogWarning("Patient with ID {PatientId} not found. Skipping event.", payload.PatientId);
            return;
        }

        var activeInvoice = await context.BilInvoices
            .Where(inv => inv.PatientId == patient.Id && inv.Status == "Active")
            .FirstOrDefaultAsync(cancellationToken);

        if (activeInvoice == null)
        {
            activeInvoice = new BilInvoice
            {
                PatientId = patient.Id,
                TotalAmount = 0,
                Status = "Active",
                CreatedAt = DateTime.UtcNow
            };
            context.BilInvoices.Add(activeInvoice);
            await context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Created new active invoice for PatientId {PatientId}", payload.PatientId);
        }

        var item = await context.InvItems
            .FirstOrDefaultAsync(inv => inv.Sku == payload.Sku, cancellationToken);

        if (item == null)
        {
            _logger.LogError("Inventory item with SKU {Sku} not found", payload.Sku);
            throw new InvalidOperationException($"Inventory item {payload.Sku} not found");
        }

        var existingInvoiceItem = await context.BilInvoiceItems
            .FirstOrDefaultAsync(ii => ii.InvoiceId == activeInvoice.Id && ii.SourceEventId == eventId, cancellationToken);

        if (existingInvoiceItem != null)
        {
            _logger.LogWarning("InvoiceItem for EventId {EventId} already exists. Skipping idempotent processing.", eventId);
            return;
        }

        var unitPrice = item.UnitPrice ?? 0;
        var totalPrice = unitPrice * payload.Quantity;

        var invoiceItem = new BilInvoiceItem
        {
            InvoiceId = activeInvoice.Id,
            Description = $"{payload.Reason}: {item.Name} (SKU: {payload.Sku})",
            Quantity = payload.Quantity,
            UnitPrice = unitPrice,
            TotalPrice = totalPrice,
            SourceEventId = eventId
        };

        context.BilInvoiceItems.Add(invoiceItem);

        activeInvoice.TotalAmount += totalPrice;

        await context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Added invoice item for SKU {Sku}, Quantity {Quantity}, Total: {TotalPrice}",
            payload.Sku, payload.Quantity, totalPrice);
    }
}
