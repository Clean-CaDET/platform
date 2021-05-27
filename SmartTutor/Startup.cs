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
using SmartTutor.Controllers.KeycloakAuth;
using SmartTutor.Controllers.Mappers;
using SmartTutor.Database;
using SmartTutor.InstructorModel.Instructors;
using SmartTutor.LearnerModel;
using SmartTutor.LearnerModel.Learners.Repository;
using SmartTutor.LearnerModel.Options;
using SmartTutor.ProgressModel;
using SmartTutor.ProgressModel.Feedback.Repository;
using SmartTutor.ProgressModel.Progress.Repository;
using SmartTutor.ProgressModel.Submissions.Repository;
using System;

namespace SmartTutor
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<WorkspaceOptions>(Configuration.GetSection(WorkspaceOptions.ConfigKey));

            services.AddAutoMapper(typeof(Startup));

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new LearningObjectJsonConverter());
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
            services.AddScoped<ILearnerRepository, LearnerDatabaseRepository>();

            services.AddScoped<IInstructor, VARKRecommender>();

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

                        if (Env.IsDevelopment())
                        {
                            return failedContext.Response.WriteAsync(failedContext.Exception.ToString());
                        }
                        return failedContext.Response.WriteAsync("An error occured processing your authentication.");
                    }
                };
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private string CreateConnectionStringFromEnvironment()
        {
            string server = Environment.GetEnvironmentVariable("DATABASE_HOST") ?? "localhost";
            string port = Environment.GetEnvironmentVariable("DATABASE_PORT") ?? "5432";
            string database = Environment.GetEnvironmentVariable("DATABASE_SCHEMA") ?? "smart-tutor-db";
            string user = Environment.GetEnvironmentVariable("DATABASE_USERNAME") ?? "postgres";
            string password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD") ?? "super";
            string integratedSecurity = Environment.GetEnvironmentVariable("DATABASE_INTEGRATED_SECURITY") ?? "false";
            string pooling = Environment.GetEnvironmentVariable("DATABASE_POOLING") ?? "true";

            return
                $"Server={server};Port={port};Database={database};User ID={user};Password={password};Integrated Security={integratedSecurity};Pooling={pooling};";
        }
    }
}
