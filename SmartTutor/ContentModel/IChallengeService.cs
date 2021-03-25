using SmartTutor.ContentModel.LearningObjects;

namespace SmartTutor.ContentModel
{
    public interface IChallengeService
    {
        bool CheckSubmittedChallengeCompletion(string[] sourceCode, int challengeId);
        Challenge GetChallenge(int challengeId);
    }
}
