using SmartTutor.ContentModel.LearningObjects.ChallengeModel;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;
using System.Collections.Generic;

namespace SmartTutor.ContentModel
{
    public interface IChallengeService
    {
        ChallengeEvaluation CheckChallengeCompletion(string[] sourceCode, int challengeId);
        Challenge GetChallenge(int challengeId);
        List<ChallengeHint> GetAllHints(int challengeId);
    }
}
