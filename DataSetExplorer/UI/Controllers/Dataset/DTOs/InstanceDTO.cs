using System.Collections.Generic;
using CodeModel.CaDETModel.CodeItems;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.DataSets.Model;

namespace DataSetExplorer.UI.Controllers.Dataset.DTOs
{
    public class InstanceDTO
    {
        public int Id { get; set; }
        public string CodeSnippetId { get; set; }
        public string Link { get; set; }
        public SnippetType Type { get; set; }
        public ISet<Annotation> Annotations { get; set; }
        public Dictionary<CaDETMetric, double> MetricFeatures { get; set; } 
        public List<RelatedInstance> RelatedInstances { get; set; }
        public int ProjectId { get; set; }

        public InstanceDTO(Instance instance)
        {
            Id = instance.Id;
            CodeSnippetId = instance.CodeSnippetId;
            Link = instance.Link;
            Type = instance.Type;
            Annotations = instance.Annotations;
            MetricFeatures = instance.MetricFeatures;
            RelatedInstances = instance.RelatedInstances;
        }
    }
}