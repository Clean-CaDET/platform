using System.Collections.Generic;
using System.Linq;

namespace CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer
{
    public class CohesiveParts
    {
        public List<ClassPart> Parts { get; }
        public HashSet<Access> AccessesToCut { get; }

        public CohesiveParts(HashSet<Access> accessesToCut, IEnumerable<HashSet<Access>> parts)
        {
            AccessesToCut = accessesToCut;
            Parts = parts.Select(part => new ClassPart(part)).ToList();
        }
    }
}