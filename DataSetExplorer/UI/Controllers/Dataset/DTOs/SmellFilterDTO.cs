namespace DataSetExplorer.UI.Controllers.Dataset.DTOs
{
    public class SmellFilterDTO
    {
        public CodeSmellDTO CodeSmell { get; set; }
        public MetricThresholdsDTO[] MetricsThresholds { get; set; }
    }
}
