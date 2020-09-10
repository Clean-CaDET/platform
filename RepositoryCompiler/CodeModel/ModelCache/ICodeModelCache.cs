namespace RepositoryCompiler.CodeModel.ModelCache
{
    public interface ICodeModelCache
    {
        CaDETModel.CaDETModel GetModel(string solutionName);
    }
}
