using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.UI.Controllers.Dataset.DTOs;
using FluentResults;

namespace DataSetExplorer.Core.DataSets
{
    public interface IInstanceService
    {
        Result<InstanceDTO> GetInstanceWithRelatedInstances(int id);
        Result<Instance> GetInstanceWithAnnotations(int id);
    }
}