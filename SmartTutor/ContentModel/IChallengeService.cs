namespace SmartTutor.ContentModel
{
    public interface IChallengeService
    {
        bool CheckSubmittedChallengeCompletion(string[] sourceCode, int challengeId);
    }
}
