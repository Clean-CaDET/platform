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
        /// ELOC: Effective lines of code, excluding comments, blank lines, function header and function braces.
        /// </summary>
        public int ELOC { get; set; }

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

        /// <summary>
        /// NOTC: Number of try catch blocks
        /// </summary>
        public int NOTC { get; set; }

        /// <summary>
        /// NOL: Number of loops
        /// </summary>
        public int NOL { get; set; }

        /// <summary>
        /// NOR: Number of return statements
        /// </summary>
        public int NOR { get; set; }

        /// <summary>
        /// NOC: Number of comparison operators
        /// </summary>
        public int NOC { get; set; }
    }
}