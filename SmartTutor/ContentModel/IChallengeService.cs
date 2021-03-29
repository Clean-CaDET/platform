using SmartTutor.ContentModel.LearningObjects.ChallengeModel;

namespace SmartTutor.ContentModel
{
    public interface IChallengeService
    {
        bool CheckChallengeCompletion(string[] sourceCode, int challengeId);
        Challenge GetChallenge(int challengeId);
    }
}
