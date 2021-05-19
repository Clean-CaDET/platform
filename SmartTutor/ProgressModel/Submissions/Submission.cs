using System;

namespace SmartTutor.ProgressModel.Submissions
{
    public class Submission
    {
        public int Id { get; private set; }
        public int LearnerId { get; private set; }
        public bool IsCorrect { get; private set; }
        public DateTime TimeStamp { get; private set; } = DateTime.Now;

        public void MarkCorrect()
        {
            IsCorrect = true;
        }

        protected Submission()
        {
        }

        public Submission(int id, int learnerId, bool isCorrect, DateTime timeStamp)
        {
            Id = id;
            LearnerId = learnerId;
            IsCorrect = isCorrect;
            TimeStamp = timeStamp;
        }

        protected Submission(Submission submission)
        {
            Id = submission.Id;
            LearnerId = submission.LearnerId;
            IsCorrect = submission.IsCorrect;
            TimeStamp = submission.TimeStamp;
        }
    }
}