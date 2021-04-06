using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;
using System.Collections.Generic;

namespace SmartTutor.ContentModel
{
    public interface IChallengeService
    {
        ChallengeEvaluation EvaluateSubmission(string[] sourceCode, int challengeId);
        List<ChallengeHint> GetAllHints(int challengeId);
    }
}
