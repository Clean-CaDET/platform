using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.Core.DataSets.Repository;
using FluentResults;

namespace DataSetExplorer.Core.DataSets
{
    public class GraphInstanceService : IGraphInstanceService
    {
        private readonly IGraphInstanceRepository _graphInstanceRepository;
        private readonly IInstanceRepository _instanceRepository;

        public GraphInstanceService(IGraphInstanceRepository graphInstanceRepository, IInstanceRepository instanceRepository)
        {
            _graphInstanceRepository = graphInstanceRepository;
            _instanceRepository = instanceRepository;  
        }

        public Result<GraphInstance> GetGraphInstanceWithRelatedInstances(int projectId, int instanceId)
        {
            var instance = _instanceRepository.Get(instanceId);
            var graphInstance = _graphInstanceRepository.GetGraphInstanceWithRelatedInstances(projectId, instance.CodeSnippetId);
            if (graphInstance == default) return Result.Fail($"Project with id: {projectId} or instance with id: {instanceId} does not exist.");
            return Result.Ok(graphInstance);
        }

        public Result<GraphInstance> GetGraphInstanceWithRelatedInstances(int projectId, string instanceCodeSnippetId)
        {
            var graphInstance = _graphInstanceRepository.GetGraphInstanceWithRelatedInstances(projectId, instanceCodeSnippetId);
            if (graphInstance == default) return Result.Fail($"Project with id: {projectId} or instance with codeSnippetId: {instanceCodeSnippetId} does not exist.");
            return Result.Ok(graphInstance);
        }
    }
}