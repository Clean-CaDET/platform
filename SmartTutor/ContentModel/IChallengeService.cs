using SmartTutor.ContentModel.LearningObjects.Challenges;

namespace SmartTutor.ContentModel
{
    public interface IChallengeService
    {
        ChallengeEvaluation EvaluateSubmission(string[] sourceCode, int challengeId, string traineeId);
    }
}
