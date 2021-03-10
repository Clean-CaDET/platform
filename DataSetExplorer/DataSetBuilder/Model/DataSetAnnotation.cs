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
            InstanceSmell = new CodeSmell(instanceSmell);
            Severity = severity;
            AnnotatorId = annotatorId;
            ApplicableHeuristics = applicableHeuristics;
            Validate();
        }

        private void Validate()
        {
            if (Severity < 0 || Severity > 3) throw new InvalidOperationException("Accepted severity ranges from 0 to 3, but was " + Severity);
            if (AnnotatorId == 0) throw new InvalidOperationException("Annotator ID is required.");
            if (Severity > 0 && ApplicableHeuristics.Count < 1) throw new InvalidOperationException("Annotations made by " + AnnotatorId + " with severity " + Severity + " must have at least one applicable heuristic.");
        }

        public override int GetHashCode() => (AnnotatorId, InstanceSmell: InstanceSmell.Value).GetHashCode();

        public override bool Equals(object other)
        {
            return other is DataSetAnnotation annotation
                   && AnnotatorId.Equals(annotation.AnnotatorId)
                   && InstanceSmell.Value.Equals(annotation.InstanceSmell.Value);
        }

        public override string ToString()
        {
            return "Annotator: " + AnnotatorId + "; Smell: " + InstanceSmell.Value + "; Severity: " + Severity;
        }
    }
}