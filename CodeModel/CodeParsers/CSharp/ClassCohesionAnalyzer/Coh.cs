using System.Linq;

namespace CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer
{
    public class Coh : ICohesionMetric
    {
        public double Calculate(ClassPart classPart)
        {
            var accesses = classPart.Accesses;
            if (accesses.Count == 0) return 1;

            int methods = accesses.GroupBy(e => e.Method).Count();
            int fields = accesses.GroupBy(e => e.Field).Count();
            return 1 - (double)accesses.Count / (methods * fields);
        }
    }
}