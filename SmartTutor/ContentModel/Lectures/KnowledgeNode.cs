using System.Collections.Generic;

namespace SmartTutor.ContentModel.Lectures
{
    public class KnowledgeNode
    {
        public int Id { get; private set; }
        public string LearningObjective { get; private set; }
        public KnowledgeNodeType Type { get; private set; }
        public List<LearningObjectSummary> LearningObjectSummaries { get; private set; }

        protected KnowledgeNode() {}
        public KnowledgeNode(int id, List<LearningObjectSummary> summaries): this()
        {
            Id = id;
            LearningObjectSummaries = summaries;
        }
    }

    public enum KnowledgeNodeType
    {
        Factual,
        Procedural,
        Conceptual
    }
}