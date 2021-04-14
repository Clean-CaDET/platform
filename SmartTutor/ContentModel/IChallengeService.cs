using SmartTutor.ContentModel.LearningObjects.ChallengeModel;

namespace SmartTutor.ContentModel
{
    public interface IChallengeService
    {
        ChallengeEvaluation EvaluateSubmission(string[] sourceCode, int challengeId, string traineeId);
    }
}
