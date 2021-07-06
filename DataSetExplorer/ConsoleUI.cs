using System;
using System.Collections.Generic;
using DataSetExplorer.DataSetBuilder.Model;
using DataSetExplorer.RepositoryAdapters;
using System.Collections.Specialized;
using System.IO;
using FluentResults;

namespace DataSetExplorer
{
    class ConsoleUI
    {
        private static IDataSetAnalysisService _dataSetAnalysisService;        

        public ConsoleUI(IDataSetAnalysisService dataSetAnalysisService)
        {
            _dataSetAnalysisService = dataSetAnalysisService;
        }

        public void Run()
        {
            string chosenOption;
            do
            {
                WriteMenuWithOptions();
                chosenOption = GetAnswerOnQuestion("Your option: ");
                Console.Clear();
                ProcessChosenOption(chosenOption);

            } while (!chosenOption.Equals("x"));
        }

        private static void ProcessChosenOption(string chosenOption)
        {
            switch (chosenOption)
            {
                case "1":
                    CreateDataSet();
                    break;
                case "2":
                    AnalyzeDataSet();
                    break;
                case "3":
                    ExportDataSet();
                    break;
                case "4":
                    CheckAnnotationsConsistency();
                    break;
                case "x":
                    break;
                default:
                    ChosenOptionErrorMessage(chosenOption);
                    break;
            }
        }

        private static void CreateDataSet()
        {
            string outputPath = GetAnswerOnQuestion("Enter output folder path: ");
            IDataSetCreationService _dataSetCreationService = new DataSetCreationService(outputPath, new GitCodeRepository());

            var projects = GetProjectsForDataSet();
            Result<string> result;
            foreach (var projectName in projects.Keys)
            {
                result = _dataSetCreationService.CreateDataSetSpreadsheet(projectName.ToString(), projects[projectName].ToString());
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
                projectName = GetAnswerOnQuestion("Enter project name: ");
                projectAndCommitUrl = GetAnswerOnQuestion("Enter project and commit URL: ");
                projects.Add(projectName, projectAndCommitUrl);
                finishOption = GetAnswerOnQuestion("Finished? (y/n): ");
            } while (finishOption.Equals("n"));

            return projects;
        }

        private static void AnalyzeDataSet()
        {
            string dataSetPath;
            string outputPath;
            string finishOption;

            do
            {
                dataSetPath = GetAnswerOnQuestion("Enter data set folder path: ");
                outputPath = GetAnswerOnQuestion("Enter output folder path: ");
                ChooseAnalyzeDataSetOption(dataSetPath, outputPath);
                finishOption = GetAnswerOnQuestion("Finished? (y/n): ");
            } while (finishOption.Equals("n"));
        }

        private static void ChooseAnalyzeDataSetOption(string dataSetPath, string outputPath)
        {
            string chosenOption;
            do
            {
                WriteAnalyzeDataSetOptions();
                chosenOption = GetAnswerOnQuestion("Your option: ");

                Result<string> result;
                switch (chosenOption)
                {
                    case "1":
                        result = _dataSetAnalysisService.FindInstancesRequiringAdditionalAnnotation(dataSetPath, outputPath);
                        Console.WriteLine(result.ToString());
                        break;
                    case "2":
                        result = _dataSetAnalysisService.FindInstancesWithAllDisagreeingAnnotations(dataSetPath, outputPath);
                        Console.WriteLine(result.ToString());
                        break;
                    case "x":
                        break;
                    default:
                        ChosenOptionErrorMessage(chosenOption);
                        break;
                }
            } while (!chosenOption.Equals("x"));
        }

        private static void ExportDataSet()
        {
            var projects = GetAnnotatedProjects();
            var annotators = GetAnnotators();
            IDataSetExportationService dataSetExportationService = new DataSetExportationService(new FullDataSetFactory(projects, annotators));

            string outputPath = GetAnswerOnQuestion("Enter output folder path: ");
            Result<string> result = dataSetExportationService.Export(outputPath);
            Console.Write(result.ToString());
        }

        private static ListDictionary GetAnnotatedProjects()
        {
            ListDictionary projects = new ListDictionary();
            string localRepoPath;
            string annotationsFolderPath;
            string finishOption;

            Console.WriteLine("Projects:");
            do
            {
                localRepoPath = GetAnswerOnQuestion("Enter local repository folder path: ");
                annotationsFolderPath = GetAnswerOnQuestion("Enter annotations folder path: ");
                projects.Add(localRepoPath, annotationsFolderPath);
                finishOption = GetAnswerOnQuestion("Finished? (y/n): ");
            } while (finishOption.Equals("n"));

            return projects;
        }

        private static List<Annotator> GetAnnotators()
        {
            List<Annotator> annotators = new List<Annotator>();
            string annotatorsFilePath = GetAnswerOnQuestion("Enter annotators file path: ");

            string[] lines = File.ReadAllLines(annotatorsFilePath);
            foreach (string line in lines)
            {
                string[] columns = line.Split(',');
                annotators.Add(new Annotator(int.Parse(columns[0]), int.Parse(columns[1]), int.Parse(columns[2])));
            }
            return annotators;
        }

        private static void CheckAnnotationsConsistency()
        {
            var projects = GetAnnotatedProjects();
            var annotators = GetAnnotators();

            IAnnotationConsistencyService annotationConsistencyService = new AnnotationConsistencyService(new FullDataSetFactory(projects, annotators));

            string chosenOption;
            do
            {
                WriteAnnotationsConsistencyOptions();
                chosenOption = GetAnswerOnQuestion("Your option: ");

                switch (chosenOption)
                {
                    case "1":
                        var annotatorId = GetId("Annotator");
                        if (annotatorId.HasValue) annotationConsistencyService.CheckAnnotationConsistencyForAnnotator(annotatorId.Value);
                        break;
                    case "2":
                        var severityId = GetId("Severity");
                        if (severityId.HasValue) annotationConsistencyService.CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(severityId.Value);
                        break;
                    case "3":
                        annotatorId = GetId("Annotator");
                        if (annotatorId.HasValue) annotationConsistencyService.CheckMetricsSignificanceInAnnotationsForAnnotator(annotatorId.Value);
                        break;
                    case "4":
                        severityId = GetId("Severity");
                        if (severityId.HasValue) annotationConsistencyService.CheckMetricsSignificanceBetweenAnnotatorsForSeverity(severityId.Value);
                        break;
                    case "x":
                        break;
                    default:
                        ChosenOptionErrorMessage(chosenOption);
                        break;
                }
            } while (!chosenOption.Equals("x"));
        }

        private static int? GetId(string name)
        {
            string id = GetAnswerOnQuestion(name + " ID: ");
            if (int.TryParse(id, out int parsedId)) return parsedId;
            Console.Write("Invalid ID: ID must be an integer.");
            return null;
        }

        private static string GetAnswerOnQuestion(string question)
        {
            Console.Write(question);
            return Console.ReadLine();
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

        private static void WriteAnalyzeDataSetOptions()
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. Find instances requiring additional annotation");
            Console.WriteLine("2. Find instances with all disagreeing annotations");
            Console.WriteLine("x. Exit\n");
        }

        private static void WriteAnnotationsConsistencyOptions()
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. Check annotation consistency for a single annotator");
            Console.WriteLine("2. Check annotation consistency between annotators");
            Console.WriteLine("3. Check metrics significance for a single annotator");
            Console.WriteLine("4. Check metrics significance between annotators");
            Console.WriteLine("x. Exit\n");
        }

        private static void ChosenOptionErrorMessage(string chosenOption)
        {
            Console.WriteLine("Option " + chosenOption + " not found. Choose again.\n");
        }
    }
}
