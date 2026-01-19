using System;
using DataSetExplorer.Core.AnnotationConsistency;
using DataSetExplorer.Core.Annotations;
using DataSetExplorer.Core.DataSets;
using DataSetExplorer.Core.DataSetSerializer;
using FluentResults;

namespace DataSetExplorer.UI.ConsoleApp
{
    class MainMenu
    {
        private readonly IDataSetExportationService _dataSetExportationService;
        private readonly IDataSetCreationService _dataSetCreationService;

        internal MainMenu(IDataSetExportationService dataSetExportationService, IDataSetCreationService dataSetCreationService)
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
                    new DataSetAnalysisSubmenu(new DataSetAnalysisService(null)).AnalyzeDataSet();
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
            string dataSetName = ConsoleIO.GetAnswerOnQuestion("Enter data set name: ");
            var codeSmells = DataSetIO.GetCodeSmells("code smell name");
            string outputPath = ConsoleIO.GetAnswerOnQuestion("Enter output folder path: ");
            var projects = DataSetIO.GetProjects("project name and project/commit URL");

            var result = _dataSetCreationService.CreateDataSetSpreadsheet(dataSetName, outputPath, projects, codeSmells);
            Console.WriteLine(result.ToString());
        }

        private void ExportDataSet()
        {
            var projects = DataSetIO.GetProjects("local repo folder and annotations folder");
            var annotators = DataSetIO.GetAnnotators();
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
