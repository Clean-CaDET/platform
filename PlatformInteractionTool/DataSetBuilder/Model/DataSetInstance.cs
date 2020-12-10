using System;
using System.Collections.Generic;

namespace PlatformInteractionTool.DataSetBuilder.Model
{
    internal class DataSetInstance
    {
        public string CodeSnippetId { get; }
        public SnippetType Type { get; }
        private ISet<DataSetAnnotation> _annotations;

        internal DataSetInstance(string codeSnippetId, SnippetType type)
        {
            CodeSnippetId = codeSnippetId;
            Type = type;
        }

        public override int GetHashCode()
        {
            return CodeSnippetId.GetHashCode();
        }

        public override bool Equals(object other)
        {
            return other is DataSetInstance instance && CodeSnippetId.Equals(instance.CodeSnippetId);
        }

        internal void AddAnnotations(DataSetInstance instance)
        {
            _annotations ??= new HashSet<DataSetAnnotation>();
            _annotations.UnionWith(instance._annotations);
        }
    }

    internal enum SnippetType
    {
        Class,
        Function
    }
}
