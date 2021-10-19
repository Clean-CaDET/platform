using DataSetExplorer.DataSetBuilder.Model;
using System.Collections.Generic;

namespace DataSetExplorer.Controllers.Dataset.DTOs
{
    public class ProjectDTO
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public List<MetricThresholds> MetricsThresholds { get; set; }
    }
}
