using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;

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

        public CaDETClassDTO(CaDETClass parsedClass)
        {
            CodeItemMetrics = new Dictionary<string, MetricsDTO>();
            Console.WriteLine("hi");

            CodeItemMetrics[parsedClass.FullName] = new MetricsDTO(parsedClass.Metrics);

            // parsedClass.FullName - identifier
            // parsedClass.Metrics - metrike
        }
    }
}