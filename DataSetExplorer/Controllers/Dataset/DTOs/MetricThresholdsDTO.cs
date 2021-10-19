namespace DataSetExplorer.Controllers.Dataset.DTOs
{
    public class MetricThresholdsDTO
    {
        public string CodeSmell { get; set; }
        public string Metric { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
    }
}
