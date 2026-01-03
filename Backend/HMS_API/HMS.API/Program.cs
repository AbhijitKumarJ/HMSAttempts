using HMS.Entity;
using HMS.Business;
using HMS.Data;
using HMS.Business.Auth;
using HMS.Business.Billing;
using HMS.Business.Bus;
using HMS.Business.Clinical;
using HMS.Business.Cpoe;
using HMS.Business.Inventory;
using HMS.Business.Patient;
using HMS.Data.Auth;
using HMS.Data.Billing;
using HMS.Data.Bus;
using HMS.Data.Clinical;
using HMS.Data.Cpoe;
using HMS.Data.Inventory;
using HMS.Data.Patient;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace HMS.API;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(new HMS.Business.Class1().GetSampleProperty());

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        });

        // Configure EF Core with Postgres connection string from appsettings.json
        builder.Services.AddDbContext<HMS.Data.DBModel.HMSContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("HMS")));


        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        
        // Legacy services (keep for backward compatibility)
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        
        // Auth module
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IAuthRepository, AuthRepository>();
        
        // Billing module
        builder.Services.AddScoped<IBillingService, BillingService>();
        builder.Services.AddScoped<IBillingRepository, BillingRepository>();
        
        // Bus module
        builder.Services.AddScoped<IBusService, BusService>();
        builder.Services.AddScoped<IBusRepository, BusRepository>();
        
        // Clinical module
        builder.Services.AddScoped<IClinicalService, ClinicalService>();
        builder.Services.AddScoped<IClinicalRepository, ClinicalRepository>();
        
        // CPOE module
        builder.Services.AddScoped<ICpoeService, CpoeService>();
        builder.Services.AddScoped<ICpoeRepository, CpoeRepository>();
        
        // Inventory module
        builder.Services.AddScoped<IInventoryService, InventoryService>();
        builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
        
        // Patient module
        builder.Services.AddScoped<IPatientService, PatientService>();
        builder.Services.AddScoped<IPatientRepository, PatientRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi("/openapi/v1.json");
        }

        app.UseStaticFiles();

        app.UseRouting();

        //app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.MapGet("/", () => Results.Redirect("/index.html"));

        app.Run();
    }
}
