using System.Collections.Generic;

namespace DataSetExplorer.Controllers.Annotations.DTOs
{
    public class CodeSmellDefinitionDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SnippetType { get; set; }
        public List<HeuristicDTO> Heuristics { get; set; }
        public SeverityRangeDTO SeverityRange { get; set; }
        public List<string> SeverityValues { get; set; }
    }
}
