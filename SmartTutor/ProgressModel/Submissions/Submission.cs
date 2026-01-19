using System;

namespace SmartTutor.ProgressModel.Submissions
{
    public abstract class Submission
    {
        public int Id { get; protected set; }
        public int LearnerId { get; protected set; }
        public bool IsCorrect { get; protected set; }
        public DateTime TimeStamp { get; private set; } = DateTime.Now;

        public void MarkCorrect()
        {
            IsCorrect = true;
        }
    }
}