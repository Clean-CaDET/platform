using DataSetExplorer.DataSetBuilder.Model;
using FluentResults;
using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer.AnnotationConsistencyTests
{
    internal interface IMetricsSignificanceTester
    {
        public Result<Dictionary<string, Dictionary<string, string>>> TestForSingleAnnotator(int annotatorId, IEnumerable<IGrouping<string, DataSetInstance>> instancesGroupedBySmells);

        public Result<Dictionary<string, Dictionary<string, string>>> TestBetweenAnnotators(IEnumerable<IGrouping<string, DataSetInstance>> instancesGroupedBySmells);
    }
}