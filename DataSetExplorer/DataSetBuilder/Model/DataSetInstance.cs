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
            var list = Annotations.Select(annotation => annotation.Annotator.Id).ToList();
            list.Sort();
            return string.Join(",", list);
        }

        public int GetFinalAnnotation()
        {
            var majorityVote = GetMajorityAnnotation();
            if (majorityVote != null) return (int)majorityVote;
            return GetAnnotationFromMostExperiencedAnnotator();
        }

        private int? GetMajorityAnnotation()
        {
            var annotationsGroupedBySeverity = Annotations.GroupBy(a => a.Severity);
            if (HasMajoritySeverityVote(annotationsGroupedBySeverity))
            {
                return annotationsGroupedBySeverity
                    .OrderByDescending(a => a.Count())
                    .First().Key;
            }
            return null;
        }

        private bool HasMajoritySeverityVote(
            IEnumerable<IGrouping<int, DataSetAnnotation>> annotationsGroupedBySeverity)
        {
            var severityCounts = annotationsGroupedBySeverity.Select(group => group.Count());
            return severityCounts.Any(count => count != severityCounts.First());
        }

        private int GetAnnotationFromMostExperiencedAnnotator()
        {
            return Annotations.OrderBy(a => a.Annotator.Ranking).First().Severity;
        }

        public bool IsAnnotatedBy(int annotatorId)
        {
            if (Annotations.Count(a => a.Annotator.Id == annotatorId) == 0) return false;
            return true;
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
