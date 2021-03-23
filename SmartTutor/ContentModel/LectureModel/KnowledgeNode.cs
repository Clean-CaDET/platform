using System.Collections.Generic;

namespace SmartTutor.ContentModel.LectureModel
{
    public class KnowledgeNode
    {
        public int Id { get; set; }
        public string Goal { get; set; }
        public List<LearningObject> LearningObjects { get; set; }
    }
}