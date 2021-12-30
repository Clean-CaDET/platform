namespace DataSetExplorer.Core.DataSets.Model
{
    public class RelatedInstance
    {
        public int Id { get; private set; }
        public string CodeSnippetId { get; private set; }
        public string Link { get; private set; }
        public RelationType RelationType { get; private set; }
        public int CouplingStrength { get; private set; }

        public RelatedInstance(string codeSnippetId, string link, RelationType relationType, int couplingStrength)
        {
            CodeSnippetId = codeSnippetId;
            Link = link;
            RelationType = relationType;
            CouplingStrength = couplingStrength;
        }

        private RelatedInstance()
        {
        }
    }
}