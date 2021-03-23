using System.Collections.Generic;

namespace SmartTutor.ContentModel.LectureModel
{
    public class Lecture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<KnowledgeNode> KnowledgeNodes { get; set; }
    }
}