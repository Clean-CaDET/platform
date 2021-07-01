using DataSetExplorer.AnnotationConsistencyTests;
using System;
using System.Linq;

namespace DataSetExplorer
{
    class AnnotationConsistencyService : IAnnotationConsistencyChecker
    {
        private IFullDataSetBuilder _fullDataSetBuilder;

        public AnnotationConsistencyService(IFullDataSetBuilder fullDataSetBuilder)
        {
            _fullDataSetBuilder = fullDataSetBuilder;
        }

        public void CheckMetricsSignificanceBetweenAnnotatorsForSeverity(int severity)
        {
            var instancesGroupedBySmells = _fullDataSetBuilder.GetAnnotatedInstancesGroupedBySmells(annotatorId: null);
            IMetricsSignificanceTester tester = new AnovaTest();
            var results = tester.TestBetweenAnnotators(severity, instancesGroupedBySmells);
            foreach (var result in results.Value)
            {
                Console.WriteLine(result.Key);
                result.Value.ToList().ForEach(pair => Console.WriteLine(pair.Key + "\n" + pair.Value));
            }
        }

        public void CheckMetricsSignificanceInAnnotationsForAnnotator(int annotatorId)
        {
            var instancesGroupedBySmells = _fullDataSetBuilder.GetAnnotatedInstancesGroupedBySmells(annotatorId);
            IMetricsSignificanceTester tester = new AnovaTest();
            var results = tester.TestForSingleAnnotator(annotatorId, instancesGroupedBySmells);
            foreach (var result in results.Value)
            {
                Console.WriteLine(result.Key);
                result.Value.ToList().ForEach(pair => Console.WriteLine(pair.Key + "\n" + pair.Value));
            }
        }

        public void CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(int severity)
        {
            var instancesGroupedBySmells = _fullDataSetBuilder.GetAnnotatedInstancesGroupedBySmells(annotatorId: null);
            IAnnotatorsConsistencyTester tester = new ManovaTest();
            var results = tester.TestConsistencyBetweenAnnotators(severity, instancesGroupedBySmells);
            results.Value.ToList().ForEach(result => Console.WriteLine(result.Key + "\n" + result.Value));
        }

        public void CheckAnnotationConsistencyForAnnotator(int annotatorId)
        {
            var instancesGroupedBySmells = _fullDataSetBuilder.GetAnnotatedInstancesGroupedBySmells(annotatorId);
            IAnnotatorsConsistencyTester tester = new ManovaTest();
            var results = tester.TestConsistencyOfSingleAnnotator(annotatorId, instancesGroupedBySmells);
            results.Value.ToList().ForEach(result => Console.WriteLine(result.Key + "\n" + result.Value));
        }
    }
}
