using Microsoft.EntityFrameworkCore;
using System;

namespace SmartTutor.LearnerModel.Learners.Workspaces
{
    [Keyless]
    public class Workspace
    {
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
