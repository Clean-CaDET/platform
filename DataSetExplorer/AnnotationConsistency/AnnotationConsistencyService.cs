using DataSetExplorer.AnnotationConsistencyTests;
using FluentResults;
using System;
using System.Linq;

namespace DataSetExplorer
{
    class AnnotationConsistencyService : IAnnotationConsistencyService
    {
        private FullDataSetFactory _fullDataSetFactory;

        public AnnotationConsistencyService(FullDataSetFactory fullDataSetFactory)
        {
            _fullDataSetFactory = fullDataSetFactory;
        }

        public Result<string> CheckMetricsSignificanceBetweenAnnotatorsForSeverity(int severity)
        {
            var instancesGroupedBySmells = _fullDataSetFactory.GetAnnotatedInstancesGroupedBySmells(annotatorId: null);
            IMetricsSignificanceTester tester = new AnovaTest();
            var results = tester.TestBetweenAnnotators(severity, instancesGroupedBySmells);
            foreach (var result in results.Value)
            {
                Console.WriteLine(result.Key);
                result.Value.ToList().ForEach(pair => Console.WriteLine(pair.Key + "\n" + pair.Value));
            }
            return Result.Ok();
        }

        public Result<string> CheckMetricsSignificanceInAnnotationsForAnnotator(int annotatorId)
        {
            var instancesGroupedBySmells = _fullDataSetFactory.GetAnnotatedInstancesGroupedBySmells(annotatorId);
            IMetricsSignificanceTester tester = new AnovaTest();
            var results = tester.TestForSingleAnnotator(annotatorId, instancesGroupedBySmells);
            foreach (var result in results.Value)
            {
                Console.WriteLine(result.Key);
                result.Value.ToList().ForEach(pair => Console.WriteLine(pair.Key + "\n" + pair.Value));
            }
            return Result.Ok();
        }

        public Result<string> CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(int severity)
        {
            var instancesGroupedBySmells = _fullDataSetFactory.GetAnnotatedInstancesGroupedBySmells(annotatorId: null);
            IAnnotatorsConsistencyTester tester = new ManovaTest();
            var results = tester.TestConsistencyBetweenAnnotators(severity, instancesGroupedBySmells);
            results.Value.ToList().ForEach(result => Console.WriteLine(result.Key + "\n" + result.Value));
            return Result.Ok();
        }

        public Result<string> CheckAnnotationConsistencyForAnnotator(int annotatorId)
        {
            var instancesGroupedBySmells = _fullDataSetFactory.GetAnnotatedInstancesGroupedBySmells(annotatorId);
            IAnnotatorsConsistencyTester tester = new ManovaTest();
            var results = tester.TestConsistencyOfSingleAnnotator(annotatorId, instancesGroupedBySmells);
            results.Value.ToList().ForEach(result => Console.WriteLine(result.Key + "\n" + result.Value));
            return Result.Ok();
        }
    }
}
