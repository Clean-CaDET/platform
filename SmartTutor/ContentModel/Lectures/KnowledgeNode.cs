using System.Collections.Generic;
using SmartTutor.ContentModel.LearningObjects;

namespace SmartTutor.ContentModel.Lectures
{
    public class KnowledgeNode
    {
        public int Id { get; private set; }
        public string LearningObjective { get; private set; }
        public KnowledgeNodeType Type { get; private set; }
        public int LectureId { get; set; }
        public List<LearningObjectSummary> LearningObjectSummaries { get; private set; }

        private KnowledgeNode()
        {
        }

        public KnowledgeNode(int id, List<LearningObjectSummary> summaries, int lectureId) : this()
        {
            Id = id;
            LearningObjectSummaries = summaries;
            LectureId = lectureId;
        }
    }

    public enum KnowledgeNodeType
    {
        Factual,
        Procedural,
        Conceptual
    }
}