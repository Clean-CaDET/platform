namespace RepositoryCompiler.CodeModel.CaDETModel
{
    public class CaDETMethod
    {
        public string Name { get; set; }
        public string SourceCode { get; set; }
        public bool IsConstructor { get; set; }
        public bool IsAccessor { get; set; }
    }
}