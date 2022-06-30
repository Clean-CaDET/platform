using System.Collections.Generic;
namespace DataSetExplorer.Core.DataSets.Model
{
    public class GraphRelatedInstance
    {
        public int Id { get; private set; }
        public string CodeSnippetId { get; private set; }
        public Dictionary<CouplingType, int> CouplingTypeAndStrength { get; private set; }

        public GraphRelatedInstance(string codeSnippetId, Dictionary<CouplingType, int> couplingTypeAndStrength)
        {
            CodeSnippetId = codeSnippetId;
            CouplingTypeAndStrength = couplingTypeAndStrength;
        }

        private GraphRelatedInstance()
        {
        }
    }
}