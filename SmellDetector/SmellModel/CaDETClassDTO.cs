using System.Collections.Generic;
using SmellDetector.Controllers;

namespace SmellDetector.SmellModel
{
    public class CaDETClassDTO
    {
        public Dictionary<string, MetricsDTO> CodeItemMetrics { get; set; }

        public CaDETClassDTO()
        {
            CodeItemMetrics = new Dictionary<string, MetricsDTO>();
        }

    }
}
