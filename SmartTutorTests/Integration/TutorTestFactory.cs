using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartTutor.Database;
using SmartTutor.Security;
using System;
using System.IO;
using System.Linq;

namespace SmartTutor.Tests.Integration
{
    public class TutorApplicationTestFactory<TStartup> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SmartTutorContext>));
                services.Remove(descriptor);
                
                services.AddDbContext<SmartTutorContext>(opt =>
                    opt.UseNpgsql(CreateConnectionStringForTest()));

                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<SmartTutorContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<TutorApplicationTestFactory<TStartup>>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        InitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }

        private static void InitializeDbForTests(SmartTutorContext db)
        {
            var startingDb = File.ReadAllText("../../../Integration/Scripts/data.sql");
            db.Database.ExecuteSqlRaw(startingDb);
        }

        private static string CreateConnectionStringForTest()
        {
            var server = Environment.GetEnvironmentVariable("DATABASE_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("DATABASE_PORT") ?? "5432";
            var database = EnvironmentConnection.GetSecret("DATABASE_SCHEMA") ?? "smart-tutor-test";
            var user = EnvironmentConnection.GetSecret("DATABASE_USERNAME") ?? "postgres";
            var password = EnvironmentConnection.GetSecret("DATABASE_PASSWORD") ?? "super";
            var integratedSecurity = Environment.GetEnvironmentVariable("DATABASE_INTEGRATED_SECURITY") ?? "false";
            var pooling = Environment.GetEnvironmentVariable("DATABASE_POOLING") ?? "true";

            return
                $"Server={server};Port={port};Database={database};User ID={user};Password={password};Integrated Security={integratedSecurity};Pooling={pooling};";
        }
    }
}
