using FluentResults;
using System;
using DataSetExplorer.Annotations;

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
            string chosenOption;
            do
            {
                WriteAnalyzeDataSetOptions();
                chosenOption = ConsoleIO.GetAnswerOnQuestion("Your option: ");

                Result<string> result;
                switch (chosenOption)
                {
                    case "1":
                        var projects = DataSetIO.GetProjects("annotated project folder path and project output folder path");
                        result = _dataSetAnalysisService.FindInstancesRequiringAdditionalAnnotation(projects);
                        Console.WriteLine(result.ToString());
                        break;
                    case "2":
                        projects = DataSetIO.GetProjects("annotated project folder path and project output folder path");
                        result = _dataSetAnalysisService.FindInstancesWithAllDisagreeingAnnotations(projects);
                        Console.WriteLine(result.ToString());
                        break;
                    case "3":
                        projects = DataSetIO.GetProjects("project/commit URL and local repo folder path");
                        var datasetPath = ConsoleIO.GetAnswerOnQuestion("Enter dataset path: ");
                        var outputFolder = ConsoleIO.GetAnswerOnQuestion("Enter output folder path: ");
                        result = _dataSetAnalysisService.ExportMembersFromAnnotatedClasses(projects, datasetPath, outputFolder);
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
            Console.WriteLine("3. Export members for annotated classes");
            Console.WriteLine("x. Exit\n");
        }
    }
}