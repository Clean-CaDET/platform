using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;

namespace RepositoryCompiler.Controllers.DTOs
{
    public class ClassMetricsDTO
    {
        public string FullName { get; set; }
        public string LOC { get; set; }
        public string LCOM { get; set; }
        public string NMD { get; set; }
        public string NAD { get; set; }
        public string WMC { get; set; }

        public ClassMetricsDTO(CaDETClass parsedClass)
        {
            FullName = parsedClass.FullName;
            LCOM = parsedClass.Metrics.LCOM.ToString();
            LOC = parsedClass.Metrics.LOC.ToString();
            NMD = parsedClass.Metrics.NMD.ToString();
            NAD = parsedClass.Metrics.NAD.ToString();
            WMC = parsedClass.Metrics.WMC.ToString();
        }
    }
}
