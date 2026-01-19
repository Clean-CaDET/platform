using Microsoft.AspNetCore.Http;

namespace DataSetExplorer.UI.Controllers.Dataset.DTOs
{
    public class ImportProjectsDTO
    {
        public IFormFile File { get; set; }
        public SmellFilterDTO[] SmellFilters { get; set; }
    }
}
