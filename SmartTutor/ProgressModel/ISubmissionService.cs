using SmartTutor.ContentModel.LearningObjects.Challenges;

namespace SmartTutor.ProgressModel
{
    public interface ISubmissionService
    {
        ChallengeEvaluation EvaluateChallenge(string[] sourceCode, int challengeId, string traineeId);
    }
}
