using System.Collections.Generic;
using DataSetExplorer.Core.DataSets.Model;
using FluentResults;

namespace DataSetExplorer.Core.AnnotationConsistency
{
    internal interface IAnnotatorsConsistencyTester
    {
        public Result<Dictionary<string, string>> TestConsistencyBetweenAnnotators(int severity, List<SmellCandidateInstances> instancesGroupedBySmells);

        public Result<Dictionary<string, string>> TestConsistencyOfSingleAnnotator(int annotatorId, List<SmellCandidateInstances> instancesGroupedBySmells); 
    }
}