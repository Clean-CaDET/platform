namespace DataSetExplorer.DataSets.Model
{
    public class MetricThresholds
    {
        public int Id { get; private set; }
        public string Metric { get; set;  }
        public string MinValue { get; set;  }
        public string MaxValue { get; set;  }

        private MetricThresholds() { }

        public MetricThresholds(string codeSmell, string metric, string minValue, string maxValue)
        {
            Metric = metric;
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}