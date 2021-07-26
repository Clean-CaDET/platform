using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartTutor.ContentModel;
using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.ContentModel.Lectures.Repository;
using SmartTutor.Controllers.Content.Mappers;
using SmartTutor.Controllers.KeycloakAuth;
using SmartTutor.Database;
using SmartTutor.InstructorModel.Instructors;
using SmartTutor.LearnerModel;
using SmartTutor.LearnerModel.Learners.Repository;
using SmartTutor.LearnerModel.Workspaces;
using SmartTutor.ProgressModel;
using SmartTutor.ProgressModel.Feedback.Repository;
using SmartTutor.ProgressModel.Progress.Repository;
using SmartTutor.ProgressModel.Submissions.Repository;
using SmartTutor.QualityAnalysis;
using SmartTutor.QualityAnalysis.Repository;
using System;
using Microsoft.Net.Http.Headers;
using System.IO;

namespace SmartTutor
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            Env = env;
        }

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Env { get; }

        private const string CorsPolicy = "_corsPolicy";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new LearningObjectJsonConverter());
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: CorsPolicy,
                    builder =>
                    {
                        builder.WithOrigins(ParseCorsOrigins())
                            .WithHeaders(HeaderNames.ContentType, HeaderNames.Authorization, "access_token")
                            .WithMethods("GET", "PUT", "POST", "DELETE", "OPTIONS");
                    });
            });

            services.AddDbContext<SmartTutorContext>(opt =>
                opt.UseNpgsql(CreateConnectionStringFromEnvironment()));

            services.AddScoped<IContentService, ContentService>();
            services.AddScoped<ILectureRepository, LectureDatabaseRepository>();
            services.AddScoped<ILearningObjectRepository, LearningObjectDatabaseRepository>();

            services.AddScoped<IProgressService, ProgressService>();
            services.AddScoped<IProgressRepository, ProgressDatabaseRepository>();
            services.AddScoped<ISubmissionService, SubmissionService>();
            services.AddScoped<ISubmissionRepository, SubmissionDatabaseRepository>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IFeedbackRepository, FeedbackDatabaseRepository>();

            services.AddScoped<ILearnerService, LearnerService>();
            services.Configure<WorkspaceOptions>(Configuration.GetSection(WorkspaceOptions.ConfigKey));
            services.AddScoped<IWorkspaceCreator, NoWorkspaceCreator>();
            services.AddScoped<ILearnerRepository, LearnerDatabaseRepository>();

            services.AddScoped<VARKRecommender, VARKRecommender>();
            services.AddScoped<IInstructor, MakeItStickRecommender>();

            services.AddScoped<ICodeQualityAnalyzer, CaDETQualityAnalyzer>();
            services.AddScoped<IAdviceRepository, AdviceDatabaseRepository>();

            if (!bool.Parse(Environment.GetEnvironmentVariable("KEYCLOAK_ON") ?? "false")) return;
            AuthenticationConfig(services);
            AuthorizationConfig(services);
        }

        private static void AuthorizationConfig(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("testPolicy", policy =>
                    policy.Requirements.Add(new KeycloakRole("Administrator")));
            });
            services.AddSingleton<IAuthorizationHandler, KeycloakRoleHandler>();
        }

        private void AuthenticationConfig(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = Environment.GetEnvironmentVariable("AUTHORITY") ?? "http://localhost:8080/auth/realms/master";
                options.Audience = Environment.GetEnvironmentVariable("AUDIENCE") ?? "demo-app";
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.Events = new JwtBearerEvents

                {
                    OnAuthenticationFailed = failedContext =>
                    {
                        failedContext.NoResult();
                        failedContext.Response.StatusCode = 500;
                        failedContext.Response.ContentType = "text/plain";

                        return failedContext.Response.WriteAsync(Env.IsDevelopment() ? 
                            failedContext.Exception.ToString() : 
                            "An error occured processing your authentication.");
                    }
                };
            });
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(CorsPolicy);

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static string GetSecret(string secretName)
        {
            var secretPath = Environment.GetEnvironmentVariable($"{secretName}_FILE") ?? "";
            return File.Exists(secretPath) ? 
                File.ReadAllText(secretPath) : 
                Environment.GetEnvironmentVariable(secretName);
        }

        private static string CreateConnectionStringFromEnvironment()
        {
            var server = Environment.GetEnvironmentVariable("DATABASE_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("DATABASE_PORT") ?? "5432";
            var database = GetSecret("DATABASE_SCHEMA") ?? "smart-tutor-db";
            var user = GetSecret("DATABASE_USERNAME") ?? "postgres";
            var password = GetSecret("DATABASE_PASSWORD") ?? "super";
            var integratedSecurity = Environment.GetEnvironmentVariable("DATABASE_INTEGRATED_SECURITY") ?? "false";
            var pooling = Environment.GetEnvironmentVariable("DATABASE_POOLING") ?? "true";

            return
                $"Server={server};Port={port};Database={database};User ID={user};Password={password};Integrated Security={integratedSecurity};Pooling={pooling};";
        }

        private static string[] ParseCorsOrigins()
        {
            string[] corsOrigins = { "http://localhost:4200" };
            var corsOriginsPath = GetSecret("SMART_TUTOR_CORS_ORIGINS");
            if (File.Exists(corsOriginsPath))
            {
                corsOrigins = File.ReadAllText(corsOriginsPath).Split(";");
            }

            return corsOrigins;
        }
    }
}