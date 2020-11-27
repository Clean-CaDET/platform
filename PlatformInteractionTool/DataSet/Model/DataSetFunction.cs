using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;

namespace PlatformInteractionTool.DataSet.Model
{
    internal class DataSetFunction
    {
        internal string FullSignature;
        internal string SourceCode;

        public DataSetFunction(CaDETMember c)
        {
            FullSignature = c.GetSignature();
            SourceCode = c.SourceCode;
        }
    }
}
