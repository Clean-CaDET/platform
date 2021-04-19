using System.Collections.Generic;
using System.Linq;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;

namespace DataSetExplorer.DataSetBuilder.Model
{
    class ExportedDataSetInstance
    {
        public string CodeSnippetId { get; }
        public string CodeSnippetLink { get; }
        public string CodeSmellType { get; set; }
        public string ProjectLink { get; }
        internal Dictionary<CaDETMetric, double> Metrics { get; set; }
        internal List<DataSetAnnotation> Annotations { get; set; }
        public static int MaxNumberOfAnnotations { get; set; }
        public static int MostExperiencedAnnotatorId { get; set; }

        public ExportedDataSetInstance(string codeSnippetId, string codeSnippetLink, string codeSmellType, string projectLink,
            int maxNumberOfAnnotations = 3, int mostExperiencedAnnotatorId = 1)
        {
            CodeSnippetId = codeSnippetId;
            CodeSnippetLink = codeSnippetLink;
            CodeSmellType = codeSmellType;
            ProjectLink = projectLink;
            MaxNumberOfAnnotations = maxNumberOfAnnotations;
            MostExperiencedAnnotatorId = mostExperiencedAnnotatorId;
        }

        public int GetAnnotationFromMostExperiencedAnnotator()
        {
            return GetAnnotationFromAnnotator(MostExperiencedAnnotatorId);
        }

        public int GetAnnotationFromAnnotator(int annotatorId)
        {
            return Annotations.Find(a => a.AnnotatorId == annotatorId).Severity;
        }

        public int GetMajorityAnnotation()
        {
            return Annotations.GroupBy(a => a.Severity)
                .OrderByDescending(a => a.Count())
                .First().Key;
        }

        public bool IsMaxAnnotated()
        {
            return Annotations.Count == MaxNumberOfAnnotations;
        }

        public bool IsAnnotatedByMostExperienced()
        {
            return IsAnnotatedBy(MostExperiencedAnnotatorId);
        }

        public bool IsAnnotatedBy(int annotatorId)
        {
            return Annotations.Exists(a => a.AnnotatorId == annotatorId);
        }
    }
}
