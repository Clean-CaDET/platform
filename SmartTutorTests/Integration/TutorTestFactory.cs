using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartTutor.Database;
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

        private void InitializeDbForTests(SmartTutorContext db)
        {
            var startingDb = File.ReadAllText("../../../Integration/Scripts/data.sql");
            db.Database.ExecuteSqlRaw(startingDb);
        }

        private string CreateConnectionStringForTest()
        {
            var server = "localhost";
            var port = "5432";
            var database = "smart-tutor-test";
            var user = "postgres";
            var password = "root";
            var integratedSecurity = "false";
            var pooling = "true";

            return
                $"Server={server};Port={port};Database={database};User ID={user};Password={password};Integrated Security={integratedSecurity};Pooling={pooling};";
        }
    }
}
