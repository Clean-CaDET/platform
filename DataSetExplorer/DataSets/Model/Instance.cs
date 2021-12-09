using CodeModel.CaDETModel.CodeItems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer.DataSets.Model
{
    public class Instance
    {
        public int Id { get; private set; }
        public string CodeSnippetId { get; private set; }
        public string Link { get; private set; }
        public string ProjectLink { get; private set; }
        public SnippetType Type { get; private set; }
        public ISet<Annotation> Annotations { get; private set; }
        public Dictionary<CaDETMetric, double> MetricFeatures { get; internal set; } // TODO: Expand and replace with the IFeature if a new feature type is introduced

        internal Instance(string codeSnippetId, string link, string projectLink, SnippetType type, Dictionary<CaDETMetric, double> metricFeatures)
        {
            CodeSnippetId = codeSnippetId;
            Link = link;
            ProjectLink = projectLink;
            Type = type;
            
            Annotations = new HashSet<Annotation>();
            SetMetricFeatures(metricFeatures);
            Validate();
        }

        private Instance()
        {
        }

        public Instance(string codeSnippetId, string projectLink)
        {
            CodeSnippetId = codeSnippetId;
            ProjectLink = projectLink;
            Annotations = new HashSet<Annotation>();
            MetricFeatures = new Dictionary<CaDETMetric, double>();
        }

        private void SetMetricFeatures(Dictionary<CaDETMetric, double> metricFeatures)
        {
            if (metricFeatures == null) return;
            foreach (var metric in metricFeatures)
            {
                if (Double.IsInfinity(metric.Value) || Double.IsNaN(metric.Value)) metricFeatures.Remove(metric.Key);
            }
            MetricFeatures = metricFeatures;
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(CodeSnippetId)) throw new ArgumentException("CodeSnippetId cannot be empty.");
        }

        internal void AddAnnotation(Annotation annotation)
        {
            Annotations.Add(annotation);
        }

        internal void AddAnnotations(Instance instance)
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
            if (Annotations.Count < 2) return false;
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
            IEnumerable<IGrouping<int, Annotation>> annotationsGroupedBySeverity)
        {
            var severityCounts = annotationsGroupedBySeverity.Select(group => group.Count());
            if (severityCounts.Count() == 1) return true;
            return severityCounts.Any(count => count != severityCounts.First());
        }

        private int GetAnnotationFromMostExperiencedAnnotator()
        {
            return Annotations.OrderBy(a => a.Annotator.Ranking).First().Severity;
        }

        public bool IsAnnotatedBy(int annotatorId)
        {
            return Annotations.Any(a => a.Annotator.Id == annotatorId);
        }

        public override int GetHashCode()
        {
            return CodeSnippetId.GetHashCode();
        }

        public override bool Equals(object other)
        {
            return other is Instance instance && CodeSnippetId.Equals(instance.CodeSnippetId);
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
