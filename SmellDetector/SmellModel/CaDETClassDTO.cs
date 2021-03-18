using System;
using System.Collections.Generic;
using SmellDetector.Controllers;

namespace SmellDetector.SmellModel
{
    public class CaDETClassDTO
    {
        public Guid Id { get; set; }

        public Dictionary<string, MetricsDTO> CodeItemMetrics { get; set; }

        public CaDETClassDTO()
        {
            CodeItemMetrics = new Dictionary<string, MetricsDTO>();
        }

    }
}
