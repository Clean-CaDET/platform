using DataSetExplorer.DataSetBuilder.Model;
using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer.AnnotationConsistencyTests
{
    internal interface IMetricsSignificanceTester
    {
        public void Test(int annotatorId, IEnumerable<IGrouping<string, DataSetInstance>> instancesGroupedBySmells);
    }
}