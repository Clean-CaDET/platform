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
            new ConsoleUI(new DataSetAnalysisService()).Run();
        }
    }
}
