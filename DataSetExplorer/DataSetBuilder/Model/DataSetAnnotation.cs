using System;
using System.Collections.Generic;

namespace DataSetExplorer.DataSetBuilder.Model
{
    public class DataSetAnnotation
    {
        public CodeSmell InstanceSmell { get; }
        public int Severity { get; }
        public List<SmellHeuristic> ApplicableHeuristics { get; }
        public int AnnotatorId { get; }

        public DataSetAnnotation(string instanceSmell, int severity, int annotatorId, List<SmellHeuristic> applicableHeuristics)
        {
            InstanceSmell = GetInstanceEnum(instanceSmell);
            Severity = severity;
            AnnotatorId = annotatorId;
            ApplicableHeuristics = applicableHeuristics;
            Validate();
        }

        private static CodeSmell GetInstanceEnum(string instanceSmell)
        {
            return instanceSmell switch
            {
                "Large Class" => CodeSmell.LargeClass,
                "Long Method" => CodeSmell.LongMethod,
                _ => throw new InvalidOperationException("Unsupported code smell type.")
            };
        }

        private void Validate()
        {
            if (Severity < 0 || Severity > 3) throw new InvalidOperationException("Severity ranges from 0 to 3.");
            if (AnnotatorId == 0) throw new InvalidOperationException("Annotator ID is required.");
            if (Severity > 0 && ApplicableHeuristics.Count < 1) throw new InvalidOperationException("Annotations with severity above 0 must have at least one applicable heuristic.");
        }

        public override int GetHashCode() => (AnnotatorId, InstanceSmell).GetHashCode();

        public override bool Equals(object other)
        {
            return other is DataSetAnnotation annotation
                   && AnnotatorId.Equals(annotation.AnnotatorId)
                   && InstanceSmell.Equals(annotation.InstanceSmell);
        }

        public override string ToString()
        {
            return "Annotator: " + AnnotatorId + "; Smell: " + InstanceSmell + "; Severity: " + Severity;
        }
    }

    public enum CodeSmell
    {
        LongMethod,
        LargeClass
    }
}