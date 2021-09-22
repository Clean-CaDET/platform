using DataSetExplorer.DataSetBuilder.Model;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer.AnnotationConsistencyTests
{
    internal interface IMetricsSignificanceTester
    {
        public Result<Dictionary<string, Dictionary<string, string>>> TestForSingleAnnotator(int annotatorId, List<CandidateDataSetInstance> instancesGroupedBySmells);

        public Result<Dictionary<string, Dictionary<string, string>>> TestBetweenAnnotators(int severity, List<CandidateDataSetInstance> instancesGroupedBySmells);
    }
}