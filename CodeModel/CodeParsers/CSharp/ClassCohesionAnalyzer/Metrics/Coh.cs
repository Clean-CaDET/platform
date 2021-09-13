using System.Linq;
using CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer.Model;

namespace CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer.Metrics
{
    public class Coh : ICohesionMetric
    {
        public double Calculate(ClassPart classPart)
        {
            var accesses = classPart.Accesses;
            if (accesses.Count == 0) return 1;

            int methods = accesses.GroupBy(e => e.Method).Count();
            int dataMembers = accesses.GroupBy(e => e.DataMember).Count();
            return 1 - ((double)accesses.Count / (methods * dataMembers));
        }
    }
}