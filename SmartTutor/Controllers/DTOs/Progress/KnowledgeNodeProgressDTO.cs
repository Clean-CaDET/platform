using SmartTutor.Controllers.DTOs.Content;
using SmartTutor.ProgressModel.Progress;
using System.Collections.Generic;

namespace SmartTutor.Controllers.DTOs.Progress
{
    public class KnowledgeNodeProgressDTO
    {
        public int Id { get; set; }
        public string LearningObjective { get; set; }
        public List<LearningObjectDTO> LearningObjects { get; set; }
        public NodeStatus Status { get; set; }
    }
}