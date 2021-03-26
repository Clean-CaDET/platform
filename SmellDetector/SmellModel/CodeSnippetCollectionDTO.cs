using System.Collections.Generic;
using SmellDetector.Controllers;

namespace SmellDetector.SmellModel
{
    public class CodeSnippetCollectionDTO
    {
        public Dictionary<string, MetricsDTO> CodeSnippetMetrics { get; set; }

        public CodeSnippetCollectionDTO()
        {
            CodeSnippetMetrics = new Dictionary<string, MetricsDTO>();
        }

    }
}
