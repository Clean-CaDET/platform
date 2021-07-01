using DataSetExplorer.DataSetBuilder.Model;
using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer
{
    interface IFullDataSetBuilder
    {
        public IEnumerable<IGrouping<string, DataSetInstance>> GetAnnotatedInstancesGroupedBySmells(int? annotatorId);
    }
}
