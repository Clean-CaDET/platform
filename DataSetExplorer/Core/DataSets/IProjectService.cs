using DataSetExplorer.Core.DataSets.Model;
using FluentResults;

namespace DataSetExplorer.Core.DataSets
{
    public interface IProjectService
    {
        Result<DataSetProject> GetProjectWithGraphInstances(int id);
    }
}