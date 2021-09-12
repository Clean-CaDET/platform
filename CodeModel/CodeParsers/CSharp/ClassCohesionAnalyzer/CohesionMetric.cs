using System.Collections.Generic;

namespace CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer
{
    public interface ICohesionMetric
    {
        double Calculate(ClassPart classPart);
    }
}