using HMS.Data.DBModel;
using Microsoft.EntityFrameworkCore;

namespace HMS.API.Tests;

public static class TestDatabaseHelper
{
    public static async Task<HMSContext> CreateTestContextAsync()
    {
        var options = new DbContextOptionsBuilder<HMSContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=hms_test;Username=postgres;Password=postgres")
            .Options;

        var context = new HMSContext(options);

        await context.Database.EnsureCreatedAsync();

        return context;
    }

    public static async Task CleanupTestDatabaseAsync(HMSContext context)
    {
        await context.AppEvents.ExecuteDeleteAsync();
        await context.SaveChangesAsync();
    }
}
