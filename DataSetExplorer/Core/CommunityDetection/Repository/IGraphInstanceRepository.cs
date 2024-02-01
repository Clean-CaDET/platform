using DataSetExplorer.Core.DataSets.Model;

namespace DataSetExplorer.Core.DataSets.Repository
{
    public interface IGraphInstanceRepository
    {
        GraphInstance GetGraphInstanceWithRelatedInstances(int projectId, string codeSnippetId);
    }
}
