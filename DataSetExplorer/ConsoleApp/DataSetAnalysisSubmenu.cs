using FluentResults;
using System;

namespace DataSetExplorer.ConsoleApp
{
    class DataSetAnalysisSubmenu
    {
        private readonly IDataSetAnalysisService _dataSetAnalysisService;

        internal DataSetAnalysisSubmenu(IDataSetAnalysisService dataSetAnalysisService)
        {
            _dataSetAnalysisService = dataSetAnalysisService;
        }

        internal void AnalyzeDataSet()
        {
            string dataSetPath;
            string outputPath;
            string finishOption;

            do
            {
                dataSetPath = ConsoleIO.GetAnswerOnQuestion("Enter data set folder path: ");
                outputPath = ConsoleIO.GetAnswerOnQuestion("Enter output folder path: ");
                ChooseAnalyzeDataSetOption(dataSetPath, outputPath);
                finishOption = ConsoleIO.GetAnswerOnQuestion("Finished? (y/n): ");
            } while (finishOption.Equals("n"));
        }

        private void ChooseAnalyzeDataSetOption(string dataSetPath, string outputPath)
        {
            string chosenOption;
            do
            {
                WriteAnalyzeDataSetOptions();
                chosenOption = ConsoleIO.GetAnswerOnQuestion("Your option: ");

                Result<string> result;
                switch (chosenOption)
                {
                    case "x":
                        break;
                    default:
                        ConsoleIO.ChosenOptionErrorMessage(chosenOption);
                        break;
                }
            } while (!chosenOption.Equals("x"));
        }

        private static void WriteAnalyzeDataSetOptions()
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. Find instances requiring additional annotation");
            Console.WriteLine("2. Find instances with all disagreeing annotations");
            Console.WriteLine("x. Exit\n");
        }
    }
}
