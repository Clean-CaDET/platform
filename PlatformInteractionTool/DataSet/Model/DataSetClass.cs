using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;

namespace PlatformInteractionTool.DataSet.Model
{
    internal class DataSetClass
    {
        internal string FullName;
        internal string SourceCode;

        public DataSetClass(CaDETClass c)
        {
            FullName = c.FullName;
            SourceCode = c.SourceCode;
        }
    }
}
