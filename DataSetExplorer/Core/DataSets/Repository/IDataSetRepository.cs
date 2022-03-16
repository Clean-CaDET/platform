using System.Collections.Generic;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.UI.Controllers.Dataset.DTOs.Summary;

namespace DataSetExplorer.Core.DataSets.Repository
{
    public interface IDataSetRepository
    {
        void Create(DataSet dataSet);
        DatasetDetailDTO GetDataSet(int id);
        DataSet GetDataSetForExport(int id);
        DataSet GetDataSetWithProjectsAndCodeSmells(int id);
        IEnumerable<DatasetSummaryDTO> GetAll();
        DataSet Update(DataSet dataSet);
        Dictionary<string, List<string>> GetDataSetCodeSmells(int id);
        DataSet Delete(int id);
    }
}
