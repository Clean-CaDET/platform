using SmartTutor.Controllers.Content.DTOs;
using SmartTutor.ProgressModel.Progress;
using System.Collections.Generic;

namespace SmartTutor.Controllers.Progress.DTOs.Progress
{
    public class KnowledgeNodeProgressDTO
    {
        public int Id { get; set; }
        public string LearningObjective { get; set; }
        public List<LearningObjectDTO> LearningObjects { get; set; }
        public NodeStatus Status { get; set; }
    }
}