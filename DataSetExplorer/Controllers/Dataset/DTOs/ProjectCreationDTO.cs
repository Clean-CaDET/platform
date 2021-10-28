namespace DataSetExplorer.Controllers.Dataset.DTOs
{
    public class ProjectCreationDTO
    {
        public ProjectDTO Project { get; set; }
        public SmellFilterDTO[] SmellFilters { get; set; }
    }
}
