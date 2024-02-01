using DataSetExplorer.Core.AnnotationSchema.Model;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.UI.Controllers.Dataset.DTOs;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer.Core.DataSets
{
    public interface IInstanceService
    {
        Result<Dictionary<string, List<Instance>>> GetInstancesWithIdentifiersByDatasetId(int datasetId);
        Result<Dictionary<string, List<Instance>>> GetInstancesWithIdentifiersByProjectId(int projectId);
        Result<Dictionary<string, List<Instance>>> GetInstancesByDatasetId(int datasetId);
        Result<Dictionary<string, List<Instance>>> GetInstancesByProjectId(int projectId);
        Result<InstanceDTO> GetInstanceWithRelatedInstances(int id);
        Result<Instance> GetInstanceWithAnnotations(int id);
        Result<List<Instance>> GetInstancesForSmell(string codeSmellName);
        Result<List<SmellCandidateInstances>> DeleteCandidateInstancesForSmell(CodeSmellDefinition codeSmellDefinition);
        public string GetFileFromGit(string url);
    }
}