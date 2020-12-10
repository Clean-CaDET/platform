using System.Collections.Generic;

namespace PlatformInteractionTool.DataSetBuilder.Model
{
    internal class DataSetAnnotation
    {
        public CodeSmell InstanceSmell { get; }
        public int Severity { get; }
        public List<SmellHeuristic> ApplicableHeuristics { get; }
        public int AnnotatorId { get; }

        public DataSetAnnotation(CodeSmell instanceSmell, int severity, int annotatorId, List<SmellHeuristic> applicableHeuristics)
        {
            InstanceSmell = instanceSmell;
            Severity = severity;
            AnnotatorId = annotatorId;
            ApplicableHeuristics = applicableHeuristics;
        }

        public override int GetHashCode()
        {
            return AnnotatorId.GetHashCode();
        }

        public override bool Equals(object other)
        {
            return other is DataSetAnnotation annotation && AnnotatorId.Equals(annotation.AnnotatorId);
        }
    }

    internal enum CodeSmell
    {
        LongMethod,
        LargeClass
    }
}