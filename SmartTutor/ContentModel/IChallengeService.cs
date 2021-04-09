using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;

namespace SmartTutor.ContentModel
{
    public interface IChallengeService
    {
        ChallengeEvaluation EvaluateSubmission(string[] sourceCode, int challengeId);
    }
}
