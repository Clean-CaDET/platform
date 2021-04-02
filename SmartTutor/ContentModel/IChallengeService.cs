using SmartTutor.ContentModel.LearningObjects.ChallengeModel;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;

namespace SmartTutor.ContentModel
{
    public interface IChallengeService
    {
        ChallengeEvaluation CheckChallengeCompletion(string[] sourceCode, int challengeId);
        Challenge GetChallenge(int challengeId);
    }
}
