using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RepositoryCompiler.CodeModel.CaDETModel.Metrics;

namespace RepositoryCompiler.Communication
{
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
            NOLV = parsedClassMetrics.NAD;             // TODO: Check is this okay ?
        }

    }
}
