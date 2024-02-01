﻿using System.Collections.Generic;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.UI.Controllers.Dataset.DTOs;

namespace DataSetExplorer.Core.DataSets.Repository
{
    public interface IInstanceRepository
    {
        Instance Get(int id);
        Dictionary<string, List<Instance>> GetInstancesWithIdentifiersByDatasetId(int datasetId);
        Dictionary<string, List<Instance>> GetInstancesWithIdentifiersByProjectId(int projectId);
        Dictionary<string, List<Instance>> GetInstancesByDatasetId(int datasetId);
        Dictionary<string, List<Instance>> GetInstancesByProjectId(int projectId);
        InstanceDTO GetInstanceWithRelatedInstances(int id);
        IEnumerable<Instance> GetInstancesAnnotatedByAnnotator(int projectId, int? annotatorId);
        IEnumerable<Instance> GetAnnotatedInstances(int projectId);
        void Update(Instance instance);
        Instance GetInstanceWithAnnotations(int id);
        List<SmellCandidateInstances> DeleteCandidateInstancesBySmell(List<CodeSmell> codeSmells);
    }
}
