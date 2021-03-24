using System.Collections.Generic;
using SmartTutor.ContentModel.TraineeModel;

namespace SmartTutor.Controllers.DTOs.Lecture
{
    public class KnowledgeNodeDTO
    {
        public int Id { get; set; }
        public string LearningObjective { get; set; }
        public List<LearningObjectDTO> LearningObjects { get; set; }
        public NodeStatus Status { get; set; }
    }
}