using DataSetExplorer.DataSetBuilder.Model;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer.AnnotationConsistencyTests
{
    internal interface IAnnotatorsConsistencyTester
    {
        public Result<Dictionary<string, string>> TestConsistencyBetweenAnnotators(int severity, List<SmellCandidateInstances> instancesGroupedBySmells);

        public Result<Dictionary<string, string>> TestConsistencyOfSingleAnnotator(int annotatorId, List<SmellCandidateInstances> instancesGroupedBySmells); 
    }
}