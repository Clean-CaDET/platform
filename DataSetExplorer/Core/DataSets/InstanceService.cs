using DataSetExplorer.Core.AnnotationSchema.Model;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.Core.DataSets.Repository;
using DataSetExplorer.UI.Controllers.Dataset.DTOs;
using FluentResults;
using System.Collections.Generic;
using System.Net.Http;

namespace DataSetExplorer.Core.DataSets
{
    public class InstanceService : IInstanceService
    {
        private readonly IInstanceRepository _instanceRepository;
        private readonly IDataSetCreationService _dataSetCreationService;
        private readonly IAnnotationRepository _annotationRepository;

        public InstanceService(IInstanceRepository instanceRepository, IDataSetCreationService dataSetCreationService,
            IAnnotationRepository annotationRepository)
        {
            _instanceRepository = instanceRepository;
            _dataSetCreationService = dataSetCreationService;
            _annotationRepository = annotationRepository;
        }

        public Result<Dictionary<string, List<Instance>>> GetInstancesWithIdentifiersByDatasetId(int datasetId)
        {
            var instances = _instanceRepository.GetInstancesWithIdentifiersByDatasetId(datasetId);
            if (instances == default) return Result.Fail($"Dataset with id: {datasetId} does not exist.");
            return Result.Ok(instances);
        }

        public Result<Dictionary<string, List<Instance>>> GetInstancesWithIdentifiersByProjectId(int projectId)
        {
            var instances = _instanceRepository.GetInstancesWithIdentifiersByProjectId(projectId);
            if (instances == default) return Result.Fail($"Project with id: {projectId} does not exist.");
            return Result.Ok(instances);
        }

        public Result<Dictionary<string, List<Instance>>> GetInstancesByDatasetId(int datasetId)
        {
            var instances = _instanceRepository.GetInstancesByDatasetId(datasetId);
            if (instances == default) return Result.Fail($"Dataset with id: {datasetId} does not exist.");
            return Result.Ok(instances);
        }

        public Result<Dictionary<string, List<Instance>>> GetInstancesByProjectId(int projectId)
        {
            var instances = _instanceRepository.GetInstancesByProjectId(projectId);
            if (instances == default) return Result.Fail($"Project with id: {projectId} does not exist.");
            return Result.Ok(instances);
        }

        public Result<InstanceDTO> GetInstanceWithRelatedInstances(int id)
        {
            var instance = _instanceRepository.GetInstanceWithRelatedInstances(id);
            if (instance == default) return Result.Fail($"Instance with id: {id} does not exist.");
            return Result.Ok(instance);
        }

        public Result<Instance> GetInstanceWithAnnotations(int id)
        {
            var instance = _instanceRepository.GetInstanceWithAnnotations(id);
            if (instance == default) return Result.Fail($"Instance with id: {id} does not exist.");
            return Result.Ok(instance);
        }

        public Result<List<Instance>> GetInstancesForSmell(string codeSmellName)
        {
            List<Instance> instances = new List<Instance>();
            var datasets = _dataSetCreationService.GetDataSetsByCodeSmell(codeSmellName).Value;
            foreach (var dataset in datasets)
            {
                foreach (var project in dataset.Projects)
                {
                    foreach (var candidate in project.CandidateInstances)
                    {
                        if (!candidate.CodeSmell.Name.Equals(codeSmellName)) continue;
                        instances.AddRange(candidate.Instances);
                    }
                }
            }
            return Result.Ok(instances);
        }

        public Result<List<SmellCandidateInstances>> DeleteCandidateInstancesForSmell(CodeSmellDefinition codeSmellDefinition)
        {
            var codeSmells = _annotationRepository.GetCodeSmellsByDefinition(codeSmellDefinition);
            var deletedCandidates = _instanceRepository.DeleteCandidateInstancesBySmell(codeSmells);
            _annotationRepository.DeleteCodeSmells(codeSmells);
            return Result.Ok(deletedCandidates);
        }

        public string GetFileFromGit(string url)
        {
            string rawUrl = "https://raw.githubusercontent.com/" + url.Split("https://github.com/")[1];
            rawUrl = rawUrl.Replace("/tree", "");

            var client = new HttpClient();
            using HttpResponseMessage response = client.GetAsync(rawUrl).Result;
            using HttpContent content = response.Content;
            var data = content.ReadAsStringAsync().Result;

            return data;
        }
    }
}