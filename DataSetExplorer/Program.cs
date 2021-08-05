using DataSetExplorer.ConsoleApp;
using DataSetExplorer.Database;
using DataSetExplorer.RepositoryAdapters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DataSetExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void CreateConsoleUI()
        {
            new MainMenu(
                new DataSetExportationService(new FullDataSetFactory()),
                new DataSetCreationService(new GitCodeRepository(), null))
                .Run();
        }
    }
}
