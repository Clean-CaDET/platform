using DataSetExplorer.Core.DataSets.Model;
using FluentResults;

namespace DataSetExplorer.Core.DataSets
{
    public interface IGraphInstanceService
    {
        Result<GraphInstance> GetGraphInstanceWithRelatedInstances(int projectId, int instanceId);
        Result<GraphInstance> GetGraphInstanceWithRelatedInstances(int projectId, string instanceCodeSnippetId);
    }
}