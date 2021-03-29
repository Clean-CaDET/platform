using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.LectureModel
{
    public class Lecture
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<KnowledgeNode> KnowledgeNodes { get; set; }
    }
}