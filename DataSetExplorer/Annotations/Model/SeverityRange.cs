namespace DataSetExplorer.Annotations.Model
{
    public class SeverityRange
    {
        public int Id { get; private set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public double Step { get; set; }

        public SeverityRange(double minValue, double maxValue, double step)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            Step = step;
        }

        private SeverityRange()
        {
        }
    }
}