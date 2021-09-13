using CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer.Model;

namespace CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer.Metrics
{
    public interface ICohesionMetric
    {
        double Calculate(ClassPart classPart);
    }
}