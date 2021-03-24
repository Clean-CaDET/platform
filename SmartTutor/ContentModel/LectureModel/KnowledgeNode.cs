using System.Collections.Generic;
using SmartTutor.ContentModel.LearningObjects;

namespace SmartTutor.ContentModel.LectureModel
{
    public class KnowledgeNode
    {
        public int Id { get; set; }
        public string LearningObjective { get; set; }
        public KnowledgeNodeType Type { get; set; }
        public List<KnowledgeNode> PrerequisiteNodes { get; set; }
        public List<LearningObjectSummary> LearningObjectSummaries { get; set; }

        public bool HasNoPrerequisites()
        {
            return PrerequisiteNodes == null || PrerequisiteNodes.Count == 0;
        }
    }

    public enum KnowledgeNodeType
    {
        Factual,
        Procedural,
        Conceptual
    }
}