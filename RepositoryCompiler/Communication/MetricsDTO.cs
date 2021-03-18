using RepositoryCompiler.CodeModel.CaDETModel.Metrics;

namespace RepositoryCompiler.Communication
{
    // TODO: Decouple class & method metrics to separate DTOs
    public class MetricsDTO
    {
        public int NOP { get; set; }

        public int CYCLO { get; set; }

        public int NOLV { get; set; }

        public int LOC { get; set; }

        public MetricsDTO() { }

        public MetricsDTO(CaDETClassMetrics parsedClassMetrics)
        {
            LOC = parsedClassMetrics.LOC;
        }

    }
}
