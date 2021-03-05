using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// TODO: Find where to store this type of dto's (shared DTO's between services)
namespace RepositoryCompiler.Communication
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