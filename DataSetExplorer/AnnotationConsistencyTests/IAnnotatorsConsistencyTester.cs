using DataSetExplorer.DataSetBuilder.Model;
using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer.AnnotationConsistencyTests
{
    internal interface IAnnotatorsConsistencyTester
    {
        public void TestConsistencyBetweenAnnotators(int severity, IEnumerable<IGrouping<string, DataSetInstance>> instancesGroupedBySmells);

        public void TestConsistencyOfSingleAnnotator(int annotatorId, IEnumerable<IGrouping<string, DataSetInstance>> instancesGroupedBySmells); 
    }
}