namespace CodeModel.CaDETModel.CodeItems
{
    public enum CaDETMetric
    {
        #region ClassMetrics

        /// <summary>
        /// CLOC: Lines of code of a class
        /// </summary>
        CLOC,

        /// <summary>
        /// CELOC: Effective lines of code of a class
        /// </summary>
        CELOC,

        /// <summary>
        /// LCOM: Lack of cohesion of methods
        /// </summary>
        LCOM,

        /// <summary>
        /// LCOM: Lack of cohesion of methods
        /// </summary>
        LCOM3,

        /// <summary>
        /// LCOM: Lack of cohesion of methods
        /// </summary>
        LCOM4,

        /// <summary>
        /// NMD: Number of methods declared
        /// </summary>
        NMD,

        /// <summary>
        /// NAD: Number of attributes defined
        /// </summary>
        NAD,

        /// <summary>
        /// Number of methods and attributes
        /// </summary>
        NMD_NAD,

        /// <summary>
        /// WMC: Weighted methods per class
        /// </summary>
        WMC,

        /// <summary>
        /// WMC: Weighted methods per class, relying on Cyclomatic complexity that doesn't count Cases
        /// </summary>
        WMC_NO_CASE,

        /// <summary>
        /// ATFD: Access to foreign data
        /// </summary>
        ATFD,

        /// <summary>
        /// ATFD: Access to foreign data directly
        /// </summary>
        ATFD_10,

        /// <summary>
        /// TCC: Tight class cohesion
        /// </summary>
        TCC,

        /// <summary>
        /// CNOR: Total number of return statements from all class members
        /// </summary>
        CNOR,

        /// <summary>
        /// CNOL: Total number of loops from all class members
        /// </summary>
        CNOL,

        /// <summary>
        /// CNOC: Total number of comparison operators from all class members
        /// </summary>
        CNOC,

        /// <summary>
        /// CNOA: Total number of assignments from all class members
        /// </summary>
        CNOA,

        /// <summary>
        /// NOPM: Number of private methods
        /// </summary>
        NOPM,

        /// <summary>
        /// NOPF: Number of protected fields
        /// </summary>
        NOPF,

        /// <summary>
        /// DIT: Depth inheritance hierarchy
        /// </summary>
        DIT,

        /// <summary>
        /// DCC: Direct class coupling
        /// </summary>
        DCC,

        /// <summary>
        /// CMNB: Max nested blocks from all class members
        /// </summary>
        CMNB,

        /// <summary>
        /// RFC: Response for a class, counts the number of unique method invocations
        /// </summary>
        RFC,

        /// <summary>
        /// CBO: Number of dependencies
        /// </summary>
        CBO,

        /// <summary>
        /// NIC: Number of inner classes
        /// </summary>
        NIC,

        #endregion

        #region MemberMetrics
        /// <summary>
        /// CYCLO - Cyclomatic complexity
        ///
        /// Also used as: VG, CC
        /// McCabe's complexity
        /// </summary>
        CYCLO,

        /// <summary>
        /// CYCLO - Cyclomatic complexity
        ///
        /// This metric doesn't count every case label, only switch label
        /// </summary>
        CYCLO_SWITCH,

        /// <summary>
        /// MLOC: Lines of code in a method
        /// </summary>
        MLOC,

        /// <summary>
        /// MELOC: Effective lines of code of a method, excluding comments, blank lines, function header and function braces.
        /// </summary>
        MELOC,

        /// <summary>
        /// NOP - number of parameters
        ///
        /// Also used as: PAR, PL
        /// </summary>
        NOP,

        /// <summary>
        /// NOLV: Number of local variables
        ///
        /// Also used as : LVAR
        /// </summary>
        NOLV,

        /// <summary>
        /// NOTC: Number of try catch blocks
        /// </summary>
        NOTC,

        /// <summary>
        /// NOL: Number of loops in method
        /// </summary>
        MNOL,

        /// <summary>
        /// NOR: Number of return statements in method
        /// </summary>
        MNOR,

        /// <summary>
        /// MNOC: Number of comparison operators in method
        /// </summary>
        MNOC,

        /// <summary>
        /// MNOA: Number of assignments in method
        /// </summary>
        MNOA,

        /// <summary>
        /// NONL: Number of numeric literals
        /// </summary>
        NONL,

        /// <summary>
        /// NOSL: Number of string literals
        /// </summary>
        NOSL,

        /// <summary>
        /// NOMO: Number of math operations
        /// </summary>
        NOMO,

        /// <summary>
        /// NOPE: Number of parenthesized expressions
        /// </summary>
        NOPE,

        /// <summary>
        /// NOLE: Number of lambda expressions
        /// </summary>
        NOLE,

        /// <summary>
        /// MMNB: Max nested blocks in the method
        /// </summary>
        MMNB,

        /// <summary>
        /// NOUW: Number of unique words
        /// </summary>
        NOUW
        #endregion
    }
}