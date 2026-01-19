using DataSetExplorer.Core.DataSets.Model;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer.Core.DataSets
{
    public interface IProjectService
    {
        Result<DataSetProject> GetProjectWithGraphInstances(int id);
        Result<List<DataSetProject>> GetAllByDatasetId(int datasetId);
    }
}