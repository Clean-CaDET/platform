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
            new ConsoleUI(new DataSetAnalysisService(), 
                new DataSetExportationService(new FullDataSetFactory()),
                new AnnotationConsistencyService(new FullDataSetFactory()),
                new DataSetCreationService(new GitCodeRepository()))
                .Run();
        }
    }
}
