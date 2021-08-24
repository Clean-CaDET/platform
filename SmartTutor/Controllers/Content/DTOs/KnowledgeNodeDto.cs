using System.Collections.Generic;
using SmartTutor.ContentModel.Lectures;

namespace SmartTutor.Controllers.Content.DTOs
{
    public class KnowledgeNodeDto
    {
        public int Id { get; set; }
        public string LearningObjective { get; set; }
        public KnowledgeNodeType Type { get; set; }
        public int LectureId { get; set; }
        public List<LearningObjectSummaryDto> LearningObjectSummaries { get; set; }
    }
}