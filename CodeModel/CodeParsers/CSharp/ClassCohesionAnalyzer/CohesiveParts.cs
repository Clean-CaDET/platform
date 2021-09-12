using System.Collections.Generic;
using System.Linq;

namespace CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer
{
    public class CohesiveParts
    {
        public List<ClassPart> Parts { get; }
        public HashSet<Access> AccessesToRemove { get; }

        public CohesiveParts(HashSet<Access> accessesToRemove, IEnumerable<HashSet<Access>> parts)
        {
            AccessesToRemove = accessesToRemove;
            Parts = parts.Select(part => new ClassPart(part)).ToList();
        }
    }
}