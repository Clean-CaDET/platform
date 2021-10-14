using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.Controllers.Annotations.DTOs
{
    public class DataSetAnnotationDTO
    {
        public int DataSetInstanceId { get; set; }
        public int Severity { get; set; }
        public string CodeSmell { get; set; }
        public List<SmellHeuristicDTO> ApplicableHeuristics { get; set; }
        public int AnnotatorId { get; set; }
    }
}
