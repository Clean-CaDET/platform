using DataSetExplorer.AnnotationConsistencyTests;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer
{
    class AnnotationConsistencyService : IAnnotationConsistencyCheckerService
    {
        private FullDataSetFactory _fullDataSetFactory;

        public AnnotationConsistencyService(FullDataSetFactory fullDataSetFactory)
        {
            _fullDataSetFactory = fullDataSetFactory;
        }

        public Result<Dictionary<string, Dictionary<string, string>>> CheckMetricsSignificanceBetweenAnnotatorsForSeverity(int severity)
        {
            var instancesGroupedBySmells = _fullDataSetFactory.GetAnnotatedInstancesGroupedBySmells(annotatorId: null);
            IMetricsSignificanceTester tester = new AnovaTest();
            return tester.TestBetweenAnnotators(severity, instancesGroupedBySmells);
        }

        public Result<Dictionary<string, Dictionary<string, string>>> CheckMetricsSignificanceInAnnotationsForAnnotator(int annotatorId)
        {
            var instancesGroupedBySmells = _fullDataSetFactory.GetAnnotatedInstancesGroupedBySmells(annotatorId);
            IMetricsSignificanceTester tester = new AnovaTest();
            return tester.TestForSingleAnnotator(annotatorId, instancesGroupedBySmells);   
        }

        public Result<Dictionary<string, string>> CheckAnnotationConsistencyBetweenAnnotatorsForSeverity(int severity)
        {
            var instancesGroupedBySmells = _fullDataSetFactory.GetAnnotatedInstancesGroupedBySmells(annotatorId: null);
            IAnnotatorsConsistencyTester tester = new ManovaTest();
            return tester.TestConsistencyBetweenAnnotators(severity, instancesGroupedBySmells);   
        }

        public Result<Dictionary<string, string>> CheckAnnotationConsistencyForAnnotator(int annotatorId)
        {
            var instancesGroupedBySmells = _fullDataSetFactory.GetAnnotatedInstancesGroupedBySmells(annotatorId);
            IAnnotatorsConsistencyTester tester = new ManovaTest();
            return tester.TestConsistencyOfSingleAnnotator(annotatorId, instancesGroupedBySmells);
        }
    }
}
