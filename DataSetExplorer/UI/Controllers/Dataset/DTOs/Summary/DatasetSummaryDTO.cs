namespace DataSetExplorer.UI.Controllers.Dataset.DTOs.Summary
{
    public class DatasetSummaryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectsCount { get; set; }

        public DatasetSummaryDTO(int id, string name, int projectsCount)
        {
            Id = id;
            Name = name;
            ProjectsCount = projectsCount;
        }
    }
}
