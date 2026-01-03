using HMS.Entity;
using HMS.Business;
using HMS.Data;
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
        builder.Services.AddDbContext<HMS.Data.DBModel.DvdRentalContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DvdRental")));


        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

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
