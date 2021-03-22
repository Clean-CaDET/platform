using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RepositoryCompiler.Communication;
using RepositoryCompiler.Controllers;
using RepositoryCompiler.RepositoryAdapters;
using IApplicationLifetime = Microsoft.AspNetCore.Hosting.IApplicationLifetime;

namespace RepositoryCompiler
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            var repo = new GitRepositoryAdapter(new Dictionary<string, string>(Configuration.GetSection("CodeRepository").AsEnumerable()));
            services.Add(new ServiceDescriptor(typeof(CodeRepositoryService), new CodeRepositoryService(repo)));

            services.AddSingleton<MessageProducer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseRabbitListener();
        }
    }

    public static class ApplicationBuilderExtentions
    {
        public static MessageProducer _producer { get; set; }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            _producer = app.ApplicationServices.GetService<MessageProducer>();

            return app;
        }

    }
}
