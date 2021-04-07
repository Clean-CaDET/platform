using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartTutor.ContentModel;
using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.ContentModel.LectureModel.Repository;
using SmartTutor.Controllers.Mappers;
using SmartTutor.Database;
using SmartTutor.Recommenders;

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
                opt.UseNpgsql(Configuration.GetConnectionString("SmartTutorConnection")));

            services.AddScoped<IContentService, ContentService>();
            services.AddScoped<IChallengeService, ChallengeService>();

            services.AddScoped<ILectureRepository, LectureDatabaseRepository>();
            services.AddScoped<ILearningObjectRepository, LearningObjectDatabaseRepository>();
            //services.AddScoped<ILectureRepository, LectureInMemoryRepository>();
            //services.AddScoped<ILearningObjectRepository, LearningObjectInMemoryRepository>();
            services.AddScoped<IRecommender, KnowledgeBasedRecommender>();
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
    }
}