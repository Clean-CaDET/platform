using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.Lectures;
using SmartTutor.LearnerModel.Learners;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SmartTutor.ProgressModel.Content
{
    public class NodeProgress
    {
        [Key] public int Id { get; set; }
        public Learner Learner { get; set; }
        public KnowledgeNode Node { get; set; }
        public List<LearningObject> LearningObjects { get; set; }
        public NodeStatus Status { get; set; }

        public void TryUnlock()
        {
            //TODO: Think about how this will be invoked.
            var completedNodes = Learner.Progress
                .Where(n => n.Status == NodeStatus.Finished)
                .Select(n => n.Node).ToList();

            foreach (var prerequisiteNode in Node.PrerequisiteNodes)
            {
                if (!completedNodes.Contains(prerequisiteNode)) return;
            }

            Status = NodeStatus.Unlocked;
        }
    }

    public enum NodeStatus
    {
        Locked,
        Unlocked,
        Started,
        Finished
    }
}