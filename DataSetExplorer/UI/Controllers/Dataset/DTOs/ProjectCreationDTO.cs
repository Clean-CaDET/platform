namespace DataSetExplorer.UI.Controllers.Dataset.DTOs
{
    public class ProjectCreationDTO
    {
        public ProjectDTO Project { get; set; }
        public SmellFilterDTO[] SmellFilters { get; set; }
        public ProjectBuildSettingsDTO BuildSettings { get; set; }
    }
}
