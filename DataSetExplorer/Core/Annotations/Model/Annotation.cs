using System;
using System.Collections.Generic;

namespace DataSetExplorer.Core.Annotations.Model
{
    public class Annotation
    {
        public int Id { get; private set; }
        public CodeSmell InstanceSmell { get; private set; }
        public int Severity { get; private set; }
        public List<SmellHeuristic> ApplicableHeuristics { get; private set; }
        public Annotator Annotator { get; set; }
        public string Note { get; private set; }

        public Annotation(string instanceSmell, int severity, Annotator annotator, List<SmellHeuristic> applicableHeuristics, String note) : 
            this(new CodeSmell(instanceSmell), severity, annotator, applicableHeuristics, note)
        {
        }

        public Annotation(CodeSmell instanceSmell, int severity, Annotator annotator, List<SmellHeuristic> applicableHeuristics, String note)
        {
            InstanceSmell = instanceSmell;
            Severity = severity;
            Annotator = annotator;
            ApplicableHeuristics = applicableHeuristics;
            Note = note;
            Validate();
        }

        private Annotation()
        {
        }

        public void Update(Annotation other)
        {
            InstanceSmell = other.InstanceSmell;
            Severity = other.Severity;
            ApplicableHeuristics = other.ApplicableHeuristics;
            Note = other.Note;
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
            return other is Annotation annotation
                   && Annotator.Equals(annotation.Annotator)
                   && InstanceSmell.Name.Equals(annotation.InstanceSmell.Name);
        }

        public override string ToString()
        {
            return "Annotator: " + Annotator.Id + "; Smell: " + InstanceSmell.Name + "; Severity: " + Severity;
        }
    }
}