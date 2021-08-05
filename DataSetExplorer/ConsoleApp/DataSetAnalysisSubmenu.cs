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

        public void AnalyzeDataSet()
        {
            var projects = DataSetIO.GetProjects("annotated project folder path and project output folder path");
            string chosenOption;
            do
            {
                WriteAnalyzeDataSetOptions();
                chosenOption = ConsoleIO.GetAnswerOnQuestion("Your option: ");

                Result<string> result;
                switch (chosenOption)
                {
                    case "1":
                        result = _dataSetAnalysisService.FindInstancesRequiringAdditionalAnnotation(projects);
                        Console.WriteLine(result.ToString());
                        break;
                    case "2":
                        result = _dataSetAnalysisService.FindInstancesWithAllDisagreeingAnnotations(projects);
                        Console.WriteLine(result.ToString());
                        break;
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
