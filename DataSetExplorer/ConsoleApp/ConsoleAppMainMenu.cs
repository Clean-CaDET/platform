using System;
using System.Collections.Specialized;
using FluentResults;

namespace DataSetExplorer.ConsoleApp
{
    class ConsoleAppMainMenu
    {
        private readonly IDataSetExportationService _dataSetExportationService;
        private readonly IDataSetCreationService _dataSetCreationService;

        internal ConsoleAppMainMenu(IDataSetExportationService dataSetExportationService, IDataSetCreationService dataSetCreationService)
        {
            _dataSetExportationService = dataSetExportationService;
            _dataSetCreationService = dataSetCreationService;
        }

        internal void Run()
        {
            string chosenOption;
            do
            {
                WriteMenuWithOptions();
                chosenOption = ConsoleIO.GetAnswerOnQuestion("Your option: ");
                Console.Clear();
                ProcessChosenOption(chosenOption);

            } while (!chosenOption.Equals("x"));
        }

        private void ProcessChosenOption(string chosenOption)
        {
            switch (chosenOption)
            {
                case "1":
                    CreateDataSet();
                    break;
                case "2":
                    new DataSetAnalysisSubmenu(new DataSetAnalysisService()).AnalyzeDataSet();
                    break;
                case "3":
                    ExportDataSet();
                    break;
                case "4":
                    new AnnotationConsistencySubmenu(new AnnotationConsistencyService(new FullDataSetFactory())).CheckAnnotationsConsistency();
                    break;
                case "x":
                    break;
                default:
                    ConsoleIO.ChosenOptionErrorMessage(chosenOption);
                    break;
            }
        }

        private void CreateDataSet()
        {
            string outputPath = ConsoleIO.GetAnswerOnQuestion("Enter output folder path: ");
            var projects = GetProjectsForDataSet();

            Result<string> result;
            foreach (var projectName in projects.Keys)
            {
                result = _dataSetCreationService.CreateDataSetSpreadsheet(outputPath, projectName.ToString(), projects[projectName].ToString());
                Console.WriteLine(result.ToString());
            }
        }

        private static ListDictionary GetProjectsForDataSet()
        {
            ListDictionary projects = new ListDictionary();
            string projectName;
            string projectAndCommitUrl;
            string finishOption;

            Console.WriteLine("\nProjects for data set:");
            do
            {
                projectName = ConsoleIO.GetAnswerOnQuestion("Enter project name: ");
                projectAndCommitUrl = ConsoleIO.GetAnswerOnQuestion("Enter project and commit URL: ");
                projects.Add(projectName, projectAndCommitUrl);
                finishOption = ConsoleIO.GetAnswerOnQuestion("Finished? (y/n): ");
            } while (finishOption.Equals("n"));

            return projects;
        }

        private void ExportDataSet()
        {
            var projects = ConsoleIO.GetAnnotatedProjects();
            var annotators = ConsoleIO.GetAnnotators();
            string outputPath = ConsoleIO.GetAnswerOnQuestion("Enter output folder path: ");
            Result<string> result = _dataSetExportationService.Export(projects, annotators, outputPath);
            Console.Write(result.ToString());
        }

        private static void WriteMenuWithOptions()
        {
            Console.WriteLine("DATASET EXPLORER\n");

            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Create dataset");
            Console.WriteLine("2. Analyze dataset");
            Console.WriteLine("3. Export dataset");
            Console.WriteLine("4. Check annotations consistency");
            Console.WriteLine("x. Exit\n");
        }        
    }
}
