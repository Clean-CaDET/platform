using DataSetExplorer.Core.DataSets.Model;
using System.Collections.Generic;

namespace DataSetExplorer.Core.DataSetSerializer
{
    public interface ICompleteDataSetExportationService
    {
        public void Export(string exportPath, List<Instance> dataSetInstances, string smell, string fileName);
    }
}
