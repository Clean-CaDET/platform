using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.Lectures;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ProgressModel.Progress
{
    public class NodeProgress
    {
        [Key] public int Id { get; set; }
        public int LearnerId { get; set; }
        public KnowledgeNode Node { get; set; }
        public List<LearningObject> LearningObjects { get; set; }
        public NodeStatus Status { get; set; }
    }

    public enum NodeStatus
    {
        Locked,
        Unlocked,
        Started,
        Finished
    }
}