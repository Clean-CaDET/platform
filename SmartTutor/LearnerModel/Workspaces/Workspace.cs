using System;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.LearnerModel.Workspaces
{
    public class Workspace
    {
        [Key]
        public int LearnerId { get; private set; }

        public string Path { get; private set; }

        public DateTime CreatedOn { get; private set; }

        public Workspace(int learnerId, string path, DateTime createdOn)
        {
            LearnerId = learnerId;
            Path = path;
            CreatedOn = createdOn;
        }
    }
}
