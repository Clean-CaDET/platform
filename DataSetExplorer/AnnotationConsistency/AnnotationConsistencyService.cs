using DataSetExplorer.AnnotationConsistencyTests;
using DataSetExplorer.DataSetBuilder.Model;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer
{
    public class AnnotationConsistencyService : IAnnotationConsistencyService
    {
        private readonly FullDataSetFactory _fullDataSetFactory;

        public AnnotationConsistencyService(FullDataSetFactory fullDataSetFactory)
        {
            _fullDataSetFactory = fullDataSetFactory;
        }

        public Result<string> CheckMetricsSignificanceBetweenAnnotatorsForSeverity(int severity, IDictionary<string, string> projects, List<Annotator> annotators)
        {
            var instancesGroupedBySmells = _fullDataSetFactory.GetAnnotatedInstancesGroupedBySmells(projects, annotators, annotatorId: null);
            IMetricsSignificanceTester tester = new AnovaTest();
            var results = tester.TestBetweenAnnotators(severity, instancesGroupedBySmells);
            foreach (var result in results.Value)
            {
                Console.WriteLine(result.Key);
                result.Value.ToList().ForEach(pair => Console.WriteLine(pair.Key + "\n" + pair.Value));
            }
            return Result.Ok();
        }

        public Result<string> CheckMetricsSignificanceInAnnotationsForAnnotator(int annotatorId, IDictionary<string, string> projects, List<Annotator> annotators)
        {
            var instancesGroupedBySmells = _fullDataSetFactory.GetAnnotatedInstancesGroupedBySmells(projects, annotators, annotatorId);
            IMetricsSignificanceTester tester = new AnovaTest();
            var results = tester.TestForSingleAnnotator(annotatorId, instancesGroupedBySmells);
            foreach (var result in results.Value)
            {
                Console.WriteLine(result.Key);
                result.Value.ToList().ForEach(pair => Console.WriteLine(pair.Key + "\n" + pair.Value));
            }
            return Result.Ok();
        }

        public Result<string> CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(int severity, IDictionary<string, string> projects, List<Annotator> annotators)
        {
            var instancesGroupedBySmells = _fullDataSetFactory.GetAnnotatedInstancesGroupedBySmells(projects, annotators, annotatorId: null);
            IAnnotatorsConsistencyTester tester = new ManovaTest();
            var results = tester.TestConsistencyBetweenAnnotators(severity, instancesGroupedBySmells);
            results.Value.ToList().ForEach(result => Console.WriteLine(result.Key + "\n" + result.Value));
            return Result.Ok();
        }

        public Result<string> CheckAnnotationConsistencyForAnnotator(int annotatorId, IDictionary<string, string> projects, List<Annotator> annotators)
        {
            var instancesGroupedBySmells = _fullDataSetFactory.GetAnnotatedInstancesGroupedBySmells(projects, annotators, annotatorId);
            IAnnotatorsConsistencyTester tester = new ManovaTest();
            var results = tester.TestConsistencyOfSingleAnnotator(annotatorId, instancesGroupedBySmells);
            results.Value.ToList().ForEach(result => Console.WriteLine(result.Key + "\n" + result.Value));
            return Result.Ok();
        }

        public Result<Dictionary<string, string>> CheckAnnotationConsistencyForAnnotator(int projectId, int annotatorId)
        {
            var instancesGroupedBySmells = _fullDataSetFactory.GetAnnotatedInstancesGroupedBySmells(projectId, annotatorId);
            IAnnotatorsConsistencyTester tester = new ManovaTest();
            return tester.TestConsistencyOfSingleAnnotator(annotatorId, instancesGroupedBySmells);
        }

        public Result<Dictionary<string, string>> CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(int projectId, int severity)
        {
            var instancesGroupedBySmells = _fullDataSetFactory.GetAnnotatedInstancesGroupedBySmells(projectId, annotatorId: null);
            IAnnotatorsConsistencyTester tester = new ManovaTest();
            return tester.TestConsistencyBetweenAnnotators(severity, instancesGroupedBySmells);
        }

        public Result<Dictionary<string, Dictionary<string, string>>> CheckMetricsSignificanceInAnnotationsForAnnotator(int projectId, int annotatorId)
        {
            var instancesGroupedBySmells = _fullDataSetFactory.GetAnnotatedInstancesGroupedBySmells(projectId, annotatorId);
            IMetricsSignificanceTester tester = new AnovaTest();
            return tester.TestForSingleAnnotator(annotatorId, instancesGroupedBySmells);
        }

        public Result<Dictionary<string, Dictionary<string, string>>> CheckMetricsSignificanceBetweenAnnotatorsForSeverity(int projectId, int severity)
        {
            var instancesGroupedBySmells = _fullDataSetFactory.GetAnnotatedInstancesGroupedBySmells(projectId, annotatorId: null);
            IMetricsSignificanceTester tester = new AnovaTest();
            return tester.TestBetweenAnnotators(severity, instancesGroupedBySmells);
        }
    }
}
