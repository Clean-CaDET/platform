using System.Collections.Generic;

namespace DataSetExplorer.DataSetBuilder.Model
{
    public class CandidateDataSetInstance
    {
        public int Id { get; private set; }
        public CodeSmell CodeSmell { get; private set; }
        public List<DataSetInstance> Instances { get; private set; }

        internal CandidateDataSetInstance(CodeSmell codeSmell, List<DataSetInstance> instances)
        {
            CodeSmell = codeSmell;
            Instances = instances;
        }

        private CandidateDataSetInstance() { }

        public bool HasInstanceWithCodeSnippetId(string codeSnippetId)
        {
            return Instances.Exists(i => i.CodeSnippetId.Equals(codeSnippetId));
        }

        public DataSetInstance GetInstanceWithCodeSnippetId(string codeSnippetId)
        {
            return Instances.Find(i => i.CodeSnippetId.Equals(codeSnippetId));
        }
    }
}
