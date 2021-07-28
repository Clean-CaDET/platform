using System.Collections.Generic;

namespace SmartTutor.ContentModel.Lectures
{
    public class Lecture
    {
        public int Id { get; set; }
        public int CourseId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public List<KnowledgeNode> KnowledgeNodes { get; private set; }

        public Lecture(int courseId, string name, string description)
        {
            CourseId = courseId;
            Name = name;
            Description = description;
            KnowledgeNodes = new List<KnowledgeNode>();
        }
    }
}