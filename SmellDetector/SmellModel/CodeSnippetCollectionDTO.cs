using System.Collections.Generic;
using SmellDetector.Controllers;

namespace SmellDetector.SmellModel
{
    public class CodeSnippetCollectionDTO
    {
        public Dictionary<string, MetricsDTO> CodeItemMetrics { get; set; }

        public CodeSnippetCollectionDTO()
        {
            CodeItemMetrics = new Dictionary<string, MetricsDTO>();
        }

    }
}
