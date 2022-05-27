using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using DataSetExplorer.Core.AnnotationConsistency;
using DataSetExplorer.Core.Annotations;
using DataSetExplorer.Core.DataSets;
using DataSetExplorer.Core.DataSets.Repository;
using DataSetExplorer.Infrastructure.Database;
using DataSetExplorer.Infrastructure.RepositoryAdapters;

namespace DataSetExplorer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);

            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();

            services.AddDbContext<DataSetExplorerContext>(opt =>
                opt.UseNpgsql(CreateConnectionStringFromEnvironment()));

            services.AddScoped<IDataSetCreationService, DataSetCreationService>();
            services.AddScoped<ICodeRepository, GitCodeRepository>();
            services.AddScoped<IDataSetRepository, DataSetDatabaseRepository>();
            services.AddScoped<IProjectRepository, ProjectDatabaseRepository>();

            services.AddScoped<IAnnotationService, AnnotationService>();
            services.AddScoped<IInstanceRepository, InstanceDatabaseRepository>();
            services.AddScoped<IAnnotationRepository, AnnotationDatabaseRepository>();

            services.AddScoped<IDataSetAnalysisService, DataSetAnalysisService>();

            services.AddScoped<IAnnotationConsistencyService, AnnotationConsistencyService>();
            services.AddScoped<FullDataSetFactory>();

            services.AddScoped<IInstanceService, InstanceService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private string CreateConnectionStringFromEnvironment()
        {
            string server = Environment.GetEnvironmentVariable("DATABASE_HOST") ?? "localhost";
            string port = Environment.GetEnvironmentVariable("DATABASE_PORT") ?? "5432";
            string database = Environment.GetEnvironmentVariable("DATABASE_SCHEMA") ?? "data-set-explorer-db";
            string user = Environment.GetEnvironmentVariable("DATABASE_USERNAME") ?? "postgres";
            string password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD") ?? "admin";
            string integratedSecurity = Environment.GetEnvironmentVariable("DATABASE_INTEGRATED_SECURITY") ?? "false";
            string pooling = Environment.GetEnvironmentVariable("DATABASE_POOLING") ?? "true";

            return
                $"Server={server};Port={port};Database={database};User ID={user};Password={password};Integrated Security={integratedSecurity};Pooling={pooling};";
        }
    }
}
