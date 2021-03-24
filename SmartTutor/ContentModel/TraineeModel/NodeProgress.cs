﻿using System.Collections.Generic;
using System.Linq;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LectureModel;

namespace SmartTutor.ContentModel.TraineeModel
{
    public class NodeProgress
    {
        public int Id { get; set; }
        public Trainee Trainee { get; set; }
        public KnowledgeNode Node { get; set; }
        public List<LearningObject> LearningObjects { get; set; }
        public NodeStatus Status { get; set; }

        public void TryUnlock()
        {
            //TODO: Think about how this will be invoked.
            var completedNodes = Trainee.Progress
                .Where(n => n.Status == NodeStatus.Finished)
                .Select(n => n.Node).ToList();
            
            foreach (var prerequisiteNode in Node.PrerequisiteNodes)
            {
                if(!completedNodes.Contains(prerequisiteNode)) return;
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