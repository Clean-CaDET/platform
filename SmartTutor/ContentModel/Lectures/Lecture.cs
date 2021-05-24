using System.Collections.Generic;

namespace SmartTutor.ContentModel.Lectures
{
    public class Lecture
    {
        public int Id { get; private set; }
        public int CourseId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public List<KnowledgeNode> KnowledgeNodes { get; private set; }
    }
}