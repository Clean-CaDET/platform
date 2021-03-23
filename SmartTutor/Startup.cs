using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartTutor.ContentModel.Repository;
using SmartTutor.Controllers.DTOs.Lecture;
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
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new LearningObjectConverter());
            });

            services.AddEntityFrameworkNpgsql().AddDbContext<ContentModelContext>(opt =>
                opt.UseNpgsql(Configuration.GetConnectionString("SmartTutorConnection")));

            services.AddScoped<IContentRepository, ContentDatabaseRepository>();
            services.AddScoped<IRecommender, KnowledgeBasedRecommender>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}