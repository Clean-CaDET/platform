namespace RepositoryCompiler.CodeModel.CaDETModel
{
    public class CaDETMember
    {
        public string Name { get; set; }
        public CaDETMemberType Type { get; set; }
        public string SourceCode { get; set; }
        public CaDETClass Parent { get; set; }
        public CaDETMemberMetrics Metrics { get; set; }
    }
}