using System.Collections.Generic;

namespace PlatformInteractionTool.DataSetBuilder.Model
{
    internal class DataSetAnnotation
    {
        public CodeSmell InstanceSmell { get; set; }
        public int Severity { get; set; }
        public List<SmellHeuristic> ApplicableHeuristics { get; set; }
        public int AnnotatorId { get; set; }
    }

    internal enum CodeSmell
    {
        LongMethod,
        LargeClass
    }
}