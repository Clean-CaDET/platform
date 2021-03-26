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
        /// Implementation based on https://github.com/mauricioaniche/ck
        /// </summary>
        public int NOTC { get; set; }

        /// <summary>
        /// NOL: Number of loops
        /// Implementation based on https://github.com/mauricioaniche/ck
        /// </summary>
        public int NOL { get; set; }

        /// <summary>
        /// NOR: Number of return statements
        /// Implementation based on https://github.com/mauricioaniche/ck
        /// </summary>
        public int NOR { get; set; }

        /// <summary>
        /// NOC: Number of comparison operators
        /// Implementation based on https://github.com/mauricioaniche/ck
        /// </summary>
        public int NOC { get; set; }

        /// <summary>
        /// NOA: Number of assignments
        /// Implementation based on https://github.com/mauricioaniche/ck
        /// </summary>
        public int NOA { get; set; }

        /// <summary>
        /// NONL: Number of numeric literals
        /// Implementation based on https://github.com/mauricioaniche/ck
        /// </summary>
        public int NONL { get; set; }

        /// <summary>
        /// NOSL: Number of string literals
        /// Implementation based on https://github.com/mauricioaniche/ck
        /// </summary>
        public int NOSL { get; set; }

        /// <summary>
        /// NOMO: Number of math operations
        /// Implementation based on https://github.com/mauricioaniche/ck
        /// </summary>
        public int NOMO { get; set; }

        /// <summary>
        /// NOPE: Number of parenthesized expressions
        /// Implementation based on https://github.com/mauricioaniche/ck
        /// </summary>
        public int NOPE { get; set; }

        /// <summary>
        /// NOLE: Number of lambda expressions
        /// Implementation based on https://github.com/mauricioaniche/ck
        /// </summary>
        public int NOLE { get; set; }

        /// <summary>
        /// MNB: Max nested blocks
        /// Implementation based on https://github.com/mauricioaniche/ck
        /// </summary>
        public int MNB { get; set; }

        /// <summary>
        /// NOUW: Number of unique words
        /// Implementation based on https://github.com/mauricioaniche/ck
        /// </summary>
        public int NOUW { get; set; }
    }
}