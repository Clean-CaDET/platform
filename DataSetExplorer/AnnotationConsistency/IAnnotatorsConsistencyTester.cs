using DataSetExplorer.DataSetBuilder.Model;
using FluentResults;
using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer.AnnotationConsistencyTests
{
    internal interface IAnnotatorsConsistencyTester
    {
        public Result<Dictionary<string, string>> TestConsistencyBetweenAnnotators(int severity, IEnumerable<IGrouping<string, DataSetInstance>> instancesGroupedBySmells);

        public Result<Dictionary<string, string>> TestConsistencyOfSingleAnnotator(int annotatorId, IEnumerable<IGrouping<string, DataSetInstance>> instancesGroupedBySmells); 
    }
}