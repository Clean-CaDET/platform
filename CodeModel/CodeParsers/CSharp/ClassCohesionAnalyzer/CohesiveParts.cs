using System.Collections.Generic;

namespace CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer
{
    public class CohesiveParts
    {
        public List<HashSet<Access>> Parts { get; }
        public HashSet<Access> AccessesToCut { get; }

        public CohesiveParts(HashSet<Access> accessesToCut, List<HashSet<Access>> parts)
        {
            AccessesToCut = accessesToCut;
            Parts = parts;
        }
    }
}