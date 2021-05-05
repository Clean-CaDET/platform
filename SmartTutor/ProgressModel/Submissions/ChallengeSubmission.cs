namespace SmartTutor.ProgressModel.Submissions
{
    public class ChallengeSubmission : Submission
    {
        public string[] SourceCode { get; private set; }
        public int ChallengeId { get; private set; }
    }
}