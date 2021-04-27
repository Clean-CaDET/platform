namespace SmartTutor.ProgressModel.Submissions
{
    public class Submission
    {
        public int Id { get; private set; }
        public int LearnerId { get; private set; }
        public bool IsCorrect { get; private set; }

        public void MarkCorrect()
        {
            IsCorrect = true;
        }
    }
}
