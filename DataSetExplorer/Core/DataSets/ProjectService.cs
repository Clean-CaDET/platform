using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.Core.DataSets.Repository;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer.Core.DataSets
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public Result<DataSetProject> GetProjectWithGraphInstances(int id)
        {
            var project = _projectRepository.GetProjectWithGraphInstances(id);
            if (project == default) return Result.Fail($"Project with id: {id} does not exist.");
            return Result.Ok(project);
        }

        public Result<List<DataSetProject>> GetAllByDatasetId(int datasetId)
        {
            return Result.Ok(_projectRepository.GetAllByDatasetId(datasetId));
        }
    }
}