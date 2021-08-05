using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.Controllers.Annotation.DTOs
{
    public class SmellHeuristicDTO
    {
        public string Description { get; set; }
        public bool IsApplicable { get; set; }
        public string ReasonForApplicability { get; set; }
    }
}
