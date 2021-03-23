namespace RepositoryCompiler.CodeModel.CaDETModel.Metrics
{
    public class CaDETClassMetrics
    {
        public int LOC { get; set; }
        public double? LCOM { get; set; }
        public int NMD { get; set; }
        public int NAD { get; set; }
        public int WMC { get; set; }
        public int ATFD { get; set; }
        public double? TCC { get; set; }

        /// <summary>
        /// NOR: Number of return statements
        /// </summary>
        public int NOR { get; set; }

        /// <summary>
        /// NOL: Number of loops
        /// </summary>
        public int NOL { get; set; }

        /// <summary>
        /// NOC: Number of comparison operators
        /// </summary>
        public int NOC { get; set; }

        /// <summary>
        /// NOA: Number of assignments
        /// </summary>
        public int NOA { get; set; }

        /// <summary>
        /// NOPM: Number of private methods
        /// </summary>
        public int NOPM { get; set; }

        /// <summary>
        /// NOPF: Number of protected fields
        /// </summary>
        public int NOPF { get; set; }

        /// <summary>
        /// MNB: Max nested blocks
        /// </summary>
        public int MNB { get; set; }
    }
}