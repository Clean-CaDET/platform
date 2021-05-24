using SmartTutor.ContentModel.LearningObjects.ArrangeTasks;
using SmartTutor.ContentModel.LearningObjects.Challenges;
using SmartTutor.ContentModel.LearningObjects.Questions;
using SmartTutor.ProgressModel.Submissions;
using System.Collections.Generic;

namespace SmartTutor.ProgressModel
{
    public interface ISubmissionService
    {
        ChallengeEvaluation EvaluateChallenge(ChallengeSubmission submission);
        List<AnswerEvaluation> EvaluateAnswers(QuestionSubmission submission);
        List<ArrangeTaskContainerEvaluation> EvaluateArrangeTask(ArrangeTaskSubmission submission);
    }
}