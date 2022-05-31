using DataSetExplorer.Core.AnnotationSchema.Model;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.UI.Controllers.Dataset.DTOs;
using FluentResults;
using System.Collections.Generic;

namespace DataSetExplorer.Core.DataSets
{
    public interface IInstanceService
    {
        Result<InstanceDTO> GetInstanceWithRelatedInstances(int id);
        Result<Instance> GetInstanceWithAnnotations(int id);
        Result<List<Instance>> GetInstancesForSmell(string codeSmellName);
    }
}