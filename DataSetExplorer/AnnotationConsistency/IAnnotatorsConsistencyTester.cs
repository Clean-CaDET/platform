using System.Collections.Generic;
using DataSetExplorer.DataSets.Model;
using FluentResults;

namespace DataSetExplorer.AnnotationConsistency
{
    internal interface IAnnotatorsConsistencyTester
    {
        public Result<Dictionary<string, string>> TestConsistencyBetweenAnnotators(int severity, List<SmellCandidateInstances> instancesGroupedBySmells);

        public Result<Dictionary<string, string>> TestConsistencyOfSingleAnnotator(int annotatorId, List<SmellCandidateInstances> instancesGroupedBySmells); 
    }
}