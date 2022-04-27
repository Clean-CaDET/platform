using System.Collections.Generic;
namespace DataSetExplorer.Core.DataSets.Model
{
    public class RelatedInstance
    {
        public int Id { get; private set; }
        public string CodeSnippetId { get; private set; }
        public string Link { get; private set; }
        public RelationType RelationType { get; private set; }
        public Dictionary<CouplingType, int> CouplingTypeAndStrength { get; private set; }

        public RelatedInstance(string codeSnippetId, string link, RelationType relationType, Dictionary<CouplingType, int> couplingTypeAndStrength)
        {
            CodeSnippetId = codeSnippetId;
            Link = link;
            RelationType = relationType;
            CouplingTypeAndStrength = couplingTypeAndStrength;
        }

        private RelatedInstance()
        {
        }
    }
}