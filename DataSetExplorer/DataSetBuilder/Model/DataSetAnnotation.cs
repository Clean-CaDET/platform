using System;
using System.Collections.Generic;

namespace DataSetExplorer.DataSetBuilder.Model
{
    public class DataSetAnnotation
    {
        public int Id { get; private set; }
        public CodeSmell InstanceSmell { get; private set; }
        public int Severity { get; private set; }
        public List<SmellHeuristic> ApplicableHeuristics { get; private set; }
        public Annotator Annotator { get; set; }

        public DataSetAnnotation(string instanceSmell, int severity, Annotator annotator, List<SmellHeuristic> applicableHeuristics) : 
            this(new CodeSmell(instanceSmell), severity, annotator, applicableHeuristics)
        {
        }

        public DataSetAnnotation(CodeSmell instanceSmell, int severity, Annotator annotator, List<SmellHeuristic> applicableHeuristics)
        {
            InstanceSmell = instanceSmell;
            Severity = severity;
            Annotator = annotator;
            ApplicableHeuristics = applicableHeuristics;
            Validate();
        }

        private DataSetAnnotation()
        {
        }

        public void Update(DataSetAnnotation other)
        {
            InstanceSmell = other.InstanceSmell;
            Severity = other.Severity;
            ApplicableHeuristics = other.ApplicableHeuristics;
            Validate();
        }

        private void Validate()
        {
            if (Severity < 0 || Severity > 3) throw new ArgumentException("Accepted severity ranges from 0 to 3, but was " + Severity);
            if (Annotator.Id == 0) throw new ArgumentException("Annotator ID is required.");
            if (Severity > 0 && ApplicableHeuristics.Count < 1) throw new ArgumentException("Annotations made by " + Annotator.Id + " with severity " + Severity + " must have at least one applicable heuristic.");
        }

        public override int GetHashCode() => (Annotator.Id, InstanceSmell: InstanceSmell.Name).GetHashCode();

        public override bool Equals(object other)
        {
            return other is DataSetAnnotation annotation
                   && Annotator.Equals(annotation.Annotator)
                   && InstanceSmell.Name.Equals(annotation.InstanceSmell.Name);
        }

        public override string ToString()
        {
            return "Annotator: " + Annotator.Id + "; Smell: " + InstanceSmell.Name + "; Severity: " + Severity;
        }
    }
}