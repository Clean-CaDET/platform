namespace CodeModel.CodeParsers.CSharp.ClassCohesionAnalyzer.Metrics
{
    public interface ICohesionMetric
    {
        double Calculate(ClassPart classPart);
    }
}