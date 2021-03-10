using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer.DataSetBuilder.Model
{
    public class DataSetInstance
    {
        public string CodeSnippetId { get; }
        public string Link { get; }
        public string ProjectLink { get; }
        public SnippetType Type { get; }
        public ISet<DataSetAnnotation> Annotations { get; }

        internal DataSetInstance(string codeSnippetId, string link, string projectLink, SnippetType type)
        {
            CodeSnippetId = codeSnippetId;
            Link = link;
            ProjectLink = projectLink;
            Type = type;

            Validate();
            Annotations = new HashSet<DataSetAnnotation>();
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(CodeSnippetId)) throw new InvalidOperationException("CodeSnippetId cannot be empty.");
        }

        internal void AddAnnotation(DataSetAnnotation annotation)
        {
            Annotations.Add(annotation);
        }

        internal void AddAnnotations(DataSetInstance instance)
        {
            if (Annotations.Overlaps(instance.Annotations)) throw new InvalidOperationException("Duplicate annotations (same author and same code quality issue): " + instance.CodeSnippetId);
            Annotations.UnionWith(instance.Annotations);
        }

        internal bool IsSufficientlyAnnotated()
        {
            if (Annotations.Count > 2) return true;
            if (Annotations.Count <= 1) return false;
            var twoVotes = Annotations.ToList();
            return twoVotes[0].Severity == twoVotes[1].Severity;
        }

        internal bool HasNoAgreeingAnnotations()
        {
            var severityGrades = Annotations.Select(a => a.Severity);
            return severityGrades.Distinct().Count() == Annotations.Count;
        }

        internal string GetSortedAnnotatorIds()
        {
            var list = Annotations.Select(annotation => annotation.AnnotatorId).ToList();
            list.Sort();
            return string.Join(",", list);
        }

        public override int GetHashCode()
        {
            return CodeSnippetId.GetHashCode();
        }

        public override bool Equals(object other)
        {
            return other is DataSetInstance instance && CodeSnippetId.Equals(instance.CodeSnippetId);
        }

        public override string ToString()
        {
            return ProjectLink + " > " + CodeSnippetId;
        }
    }

    public enum SnippetType
    {
        Class,
        Function
    }
}
