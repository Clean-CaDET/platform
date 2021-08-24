using System.Collections.Generic;

namespace SmartTutor.Controllers.Content.DTOs
{
    public class LearningObjectSummaryDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public KnowledgeNodeDto KnowledgeNode { get; set; }
        public List<LearningObjectDTO> LearningObjects { get; set; }
    }
}