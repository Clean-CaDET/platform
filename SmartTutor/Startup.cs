using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartTutor.ContentModel;
using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.ContentModel.Lectures.Repository;
using SmartTutor.Controllers.Mappers;
using SmartTutor.Database;
using SmartTutor.InstructorModel.Instructors;
using SmartTutor.LearnerModel;
using SmartTutor.LearnerModel.Learners.Repository;
using SmartTutor.ProgressModel;
using SmartTutor.ProgressModel.Feedback.Repository;
using SmartTutor.ProgressModel.Progress.Repository;
using SmartTutor.ProgressModel.Submissions.Repository;
using System;

namespace SmartTutor
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