namespace DataSetExplorer.UI.Controllers.Dataset.DTOs.Summary
{
    public class ProjectSummaryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int InstancesCount { get; set; }
        public string State { get; set; }

        public ProjectSummaryDTO(int id, string name, string url, string state, int instancesCount)
        {
            Id = id;
            Name = name;
            Url = url;
            State = state;
            InstancesCount = instancesCount;
        }
    }
}
