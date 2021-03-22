using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmellDetector.Communication;
using IApplicationLifetime = Microsoft.AspNetCore.Hosting.IApplicationLifetime;

namespace SmellDetector
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
            services.AddControllers();

            services.AddSingleton<MessageConsumer>();
            services.AddSingleton<MessageProducer>();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseRabbitListener();
        }

    }

    public static class ApplicationBuilderExtentions
    {
        public static MessageConsumer _consumer { get; set; }

        public static MessageProducer _producer { get; set; }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            //OVDE SI STAO OVDE SE KREIRA INSTANCA CONSUMERA
            _consumer = app.ApplicationServices.GetService<MessageConsumer>();

            _producer = app.ApplicationServices.GetService<MessageProducer>();

            var lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();

/*          lifetime.ApplicationStarted.Register(OnStarted);*/

            return app;
        }

/*        private static void OnStarted()
        {
            _consumer = new MessageConsumer();
            _producer = new MessageProducer();
        }*/
    }
}
