using DataSetExplorer.DataSetBuilder.Model;
using FluentResults;
using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer.AnnotationConsistencyTests
{
    internal interface IMetricsSignificanceTester
    {
        public Result<Dictionary<string, Dictionary<string, string>>> Test(int annotatorId, IEnumerable<IGrouping<string, DataSetInstance>> instancesGroupedBySmells);
    }
}