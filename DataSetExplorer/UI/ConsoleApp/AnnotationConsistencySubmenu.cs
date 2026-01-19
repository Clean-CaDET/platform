using System;
using DataSetExplorer.Core.AnnotationConsistency;

namespace DataSetExplorer.UI.ConsoleApp
{
    class AnnotationConsistencySubmenu
    {
        private readonly IAnnotationConsistencyService _annotationConsistencyService;

        internal AnnotationConsistencySubmenu(IAnnotationConsistencyService annotationConsistencyService)
        {
            _annotationConsistencyService = annotationConsistencyService;
        }

        internal void CheckAnnotationsConsistency()
        {
            var projects = DataSetIO.GetProjects("local repo folder and annotations folder");
            var annotators = DataSetIO.GetAnnotators();

            string chosenOption;
            do
            {
                WriteAnnotationsConsistencyOptions();
                chosenOption = ConsoleIO.GetAnswerOnQuestion("Your option: ");

                switch (chosenOption)
                {
                    case "1":
                        var annotatorId = GetId("Annotator");
                        if (annotatorId.HasValue) _annotationConsistencyService.CheckAnnotationConsistencyForAnnotator(annotatorId.Value, projects, annotators);
                        break;
                    case "2":
                        var severityId = GetId("Severity");
                        if (severityId.HasValue) _annotationConsistencyService.CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(severityId.Value, projects, annotators);
                        break;
                    case "3":
                        annotatorId = GetId("Annotator");
                        if (annotatorId.HasValue) _annotationConsistencyService.CheckMetricsSignificanceInAnnotationsForAnnotator(annotatorId.Value, projects, annotators);
                        break;
                    case "4":
                        severityId = GetId("Severity");
                        if (severityId.HasValue) _annotationConsistencyService.CheckMetricsSignificanceBetweenAnnotatorsForSeverity(severityId.Value, projects, annotators);
                        break;
                    case "x":
                        break;
                    default:
                        ConsoleIO.ChosenOptionErrorMessage(chosenOption);
                        break;
                }
            } while (!chosenOption.Equals("x"));
        }

        private static int? GetId(string name)
        {
            string id = ConsoleIO.GetAnswerOnQuestion(name + " ID: ");
            if (int.TryParse(id, out int parsedId)) return parsedId;
            Console.Write("Invalid ID: ID must be an integer.");
            return null;
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
    }
}
