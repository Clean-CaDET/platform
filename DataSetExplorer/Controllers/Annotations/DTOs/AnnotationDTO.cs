using System.Collections.Generic;

namespace DataSetExplorer.Controllers.Annotations.DTOs
{
    public class AnnotationDTO
    {
        public int InstanceId { get; set; }
        public int Severity { get; set; }
        public string CodeSmell { get; set; }
        public List<SmellHeuristicDTO> ApplicableHeuristics { get; set; }
        public int AnnotatorId { get; set; }
    }
}
