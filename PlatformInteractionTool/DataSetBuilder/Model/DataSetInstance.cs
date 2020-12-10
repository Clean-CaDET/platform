using System.Collections.Generic;

namespace PlatformInteractionTool.DataSetBuilder.Model
{
    internal class DataSetInstance
    {
        public string CodeSnippetId { get; set; }
        public SnippetType Type { get; set; }
        public List<DataSetAnnotation> Annotations { get; set; }

        internal DataSetInstance(string codeSnippetId, SnippetType type)
        {
            CodeSnippetId = codeSnippetId;
            Type = type;
        }
    }

    internal enum SnippetType
    {
        Class,
        Function
    }
}
