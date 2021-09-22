using DataSetExplorer.DataSetBuilder.Model;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer.AnnotationConsistencyTests
{
    internal interface IAnnotatorsConsistencyTester
    {
        public Result<Dictionary<string, string>> TestConsistencyBetweenAnnotators(int severity, List<CandidateDataSetInstance> instancesGroupedBySmells);

        public Result<Dictionary<string, string>> TestConsistencyOfSingleAnnotator(int annotatorId, List<CandidateDataSetInstance> instancesGroupedBySmells); 
    }
}