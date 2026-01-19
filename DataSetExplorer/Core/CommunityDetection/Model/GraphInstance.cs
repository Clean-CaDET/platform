using System;
using System.Collections.Generic;

namespace DataSetExplorer.Core.DataSets.Model
{
    public class GraphInstance
    {
        public int Id { get; private set; }
        public string CodeSnippetId { get; private set; }
        public string Link { get; private set; }
        public List<GraphRelatedInstance> RelatedInstances { get; private set; }

        internal GraphInstance(string codeSnippetId, string link, List<GraphRelatedInstance> relatedInstances)
        {
            CodeSnippetId = codeSnippetId;
            Link = link;
            RelatedInstances = relatedInstances;
            Validate();
        }

        private GraphInstance()
        {
        }

        public GraphInstance(string codeSnippetId)
        {
            CodeSnippetId = codeSnippetId;
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(CodeSnippetId)) throw new ArgumentException("CodeSnippetId cannot be empty.");
        }

        public override int GetHashCode()
        {
            return CodeSnippetId.GetHashCode();
        }

        public override bool Equals(object other)
        {
            return other is Instance instance && CodeSnippetId.Equals(instance.CodeSnippetId);
        }
    }
}
