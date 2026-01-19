using System.Collections.Generic;

namespace DataSetExplorer.UI.Controllers.Annotations.DTOs
{
    public class AnnotationDTO
    {
        public int InstanceId { get; set; }
        public string Severity { get; set; }
        public string CodeSmell { get; set; }
        public List<SmellHeuristicDTO> ApplicableHeuristics { get; set; }
        public int AnnotatorId { get; set; }
        public string Note { get; set; }
    }
}
