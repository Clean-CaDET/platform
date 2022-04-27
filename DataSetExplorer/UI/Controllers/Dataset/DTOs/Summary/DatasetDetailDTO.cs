using System.Collections.Generic;
using System.Linq;

namespace DataSetExplorer.UI.Controllers.Dataset.DTOs.Summary
{
    public class DatasetDetailDTO: DatasetSummaryDTO
    {
        public int InstancesCount { get; set; }
        public List<ProjectSummaryDTO> Projects { get; set; }

        public DatasetDetailDTO(int id, string name, int projectsCount,
            List<ProjectSummaryDTO> projects): base(id, name, projectsCount)
        {
            InstancesCount = projects.Sum(p => p.InstancesCount);
            Projects = projects;
        }
    }
}
