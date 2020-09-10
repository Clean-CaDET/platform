namespace RepositoryCompiler.CodeModel.CaDETModel
{
    public class CaDETDocument
    {
        public string Name { get; private set; }
        public string SourceCode { get; private set; }

        public CaDETDocument(string name, string sourceCode)
        {
            Name = name;
            SourceCode = sourceCode;
        }
    }
}