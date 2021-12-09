using System.Collections.Generic;
using DataSetExplorer.DataSets.Model;
using FluentResults;

namespace DataSetExplorer.AnnotationConsistency
{
    internal interface IMetricsSignificanceTester
    {
        public Result<Dictionary<string, Dictionary<string, string>>> TestForSingleAnnotator(int annotatorId, List<SmellCandidateInstances> instancesGroupedBySmells);

        public Result<Dictionary<string, Dictionary<string, string>>> TestBetweenAnnotators(int severity, List<SmellCandidateInstances> instancesGroupedBySmells);
    }
}