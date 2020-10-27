namespace RepositoryCompiler.CodeModel.CaDETModel.Metrics
{
    public class CaDETMemberMetrics
    {
        /// <summary>
        /// CYCLO - Cyclomatic complexity
        ///
        /// Also used as: VG, CC
        /// McCabe's complexity
        /// </summary>
        public int CYCLO { get; set; }

        /// <summary>
        /// LOC: Line of code in a method
        ///
        /// Also used as: MLOC, LOCM
        /// </summary>
        public int LOC { get; set; }

        /// <summary>
        /// NOP - number of parameters
        ///
        /// Also used as: PAR, PL
        /// </summary>
        public int NOP { get; set; }

        /// <summary>
        /// NOLV: Number of local variables
        ///
        /// Also used as : LVAR
        /// </summary>
        public int NOLV { get; set; }
    }
}