using System.Collections.Generic;

namespace SmartTutor.Controllers.DTOs.Content
{
    public class LectureDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> KnowledgeNodeIds { get; set; }
    }
}