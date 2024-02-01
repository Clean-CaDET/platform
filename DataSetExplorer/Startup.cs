﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using DataSetExplorer.Core.AnnotationConsistency;
using DataSetExplorer.Core.Annotations;
using DataSetExplorer.Core.AnnotationSchema;
using DataSetExplorer.Core.AnnotationSchema.Repository;
using DataSetExplorer.Core.DataSets;
using DataSetExplorer.Core.DataSets.Repository;
using DataSetExplorer.Infrastructure.Database;
using DataSetExplorer.Infrastructure.RepositoryAdapters;
using DataSetExplorer.Core.DataSetSerializer;
using DataSetExplorer.Core.Auth;
using DataSetExplorer.Core.Auth.Repository;
using DataSetExplorer.Core.CleanCodeAnalysis;

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
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ICodeRepository, GitCodeRepository>();
            services.AddScoped<IDataSetRepository, DataSetDatabaseRepository>();
            services.AddScoped<IProjectRepository, ProjectDatabaseRepository>();

            services.AddScoped<IAnnotationService, AnnotationService>();
            services.AddScoped<IInstanceRepository, InstanceDatabaseRepository>();
            services.AddScoped<IAnnotationRepository, AnnotationDatabaseRepository>();

            services.AddScoped<IDataSetAnalysisService, DataSetAnalysisService>();
            
            services.AddScoped<IAnnotationConsistencyService, AnnotationConsistencyService>();
            services.AddScoped<IAnnotationSchemaService, AnnotationSchemaService>();
            services.AddScoped<IAnnotationSchemaRepository, AnnotationSchemaDatabaseRepository>();
            services.AddScoped<FullDataSetFactory>();
            
            services.AddScoped<ICleanCodeAnalysisService, CleanCodeAnalysisService>();
            services.AddScoped<IInstanceService, InstanceService>();

            services.AddScoped<IGraphInstanceService, GraphInstanceService>();
            services.AddScoped<IGraphInstanceRepository, GraphInstanceRepository>();
                
            services.AddScoped<IDataSetExportationService, DataSetExportationService>();
            services.AddScoped<IDraftDataSetExportationService, DraftDataSetExportationService>();
            services.AddScoped<ICompleteDataSetExportationService, CompleteDataSetExportationService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthRepository, AuthRepository>();
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
            string password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD") ?? "super";
            string integratedSecurity = Environment.GetEnvironmentVariable("DATABASE_INTEGRATED_SECURITY") ?? "false";
            string pooling = Environment.GetEnvironmentVariable("DATABASE_POOLING") ?? "true";

            return
                $"Server={server};Port={port};Database={database};User ID={user};Password={password};Integrated Security={integratedSecurity};Pooling={pooling};";
        }
    }
}
