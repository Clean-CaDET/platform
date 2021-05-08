using SmartTutor.ContentModel.LearningObjects.ArrangeTasks;
using SmartTutor.ContentModel.LearningObjects.Challenges;
using SmartTutor.ContentModel.LearningObjects.Questions;
using SmartTutor.ProgressModel.Submissions;
using System.Collections.Generic;

namespace SmartTutor.ProgressModel
{
    public interface ISubmissionService
    {
        ChallengeEvaluation EvaluateChallenge(ChallengeSubmission submission, int learnerId);
        List<AnswerEvaluation> EvaluateAnswers(QuestionSubmission submission, int learnerId);
        List<ArrangeTaskContainerEvaluation> EvaluateArrangeTask(ArrangeTaskSubmission submission);
    }
}
