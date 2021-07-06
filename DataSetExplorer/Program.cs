using DataSetExplorer.ConsoleApp;
using DataSetExplorer.RepositoryAdapters;

namespace DataSetExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateConsoleUI();
        }

        private static void CreateConsoleUI()
        {
            new ConsoleAppMainMenu(
                new DataSetExportationService(new FullDataSetFactory()),
                new DataSetCreationService(new GitCodeRepository()))
                .Run();
        }
    }
}
