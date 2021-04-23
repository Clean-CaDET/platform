using SmartTutor.ContentModel.LearningObjects.ArrangeTasks;
using SmartTutor.ContentModel.LearningObjects.Challenges;
using SmartTutor.ContentModel.LearningObjects.Questions;
using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.ProgressModel.Submissions;
using SmartTutor.ProgressModel.Submissions.Repository;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.ProgressModel
{
    public class SubmissionService : ISubmissionService
    {
        private readonly ILearningObjectRepository _learningObjectRepository;
        private readonly ISubmissionRepository _submissionRepository;

        public SubmissionService(ILearningObjectRepository learningObjectRepository, ISubmissionRepository submissionRepository)
        {
            _learningObjectRepository = learningObjectRepository;
            _submissionRepository = submissionRepository;
        }

        public ChallengeEvaluation EvaluateChallenge(ChallengeSubmission submission)
        {
            Challenge challenge = _learningObjectRepository.GetChallenge(submission.ChallengeId);
            if (challenge == null) return null;

            var evaluation = challenge.CheckChallengeFulfillment(submission.SourceCode);

            submission.IsCorrect = evaluation.ChallengeCompleted;
            _submissionRepository.SaveChallengeSubmission(submission);

            //TODO: Tie in with Instructor and handle traineeId to get suitable LO for LO summaries.
            evaluation.ApplicableLOs =
                _learningObjectRepository.GetFirstLearningObjectsForSummaries(
                    evaluation.ApplicableHints.GetDistinctLearningObjectSummaries());
            evaluation.SolutionLO = _learningObjectRepository.GetLearningObjectForSummary(challenge.Solution.Id);
            
            return evaluation;
        }

        public List<AnswerEvaluation> EvaluateAnswers(QuestionSubmission submission)
        {
            var question = _learningObjectRepository.GetQuestion(submission.QuestionId);
            var evaluations = question.EvaluateAnswers(submission.SubmittedAnswerIds);

            submission.IsCorrect = evaluations.Select(a => a.SubmissionWasCorrect).All(c => c);
            _submissionRepository.SaveQuestionSubmission(submission);

            return evaluations;
        }

        public List<ArrangeTaskContainerEvaluation> EvaluateArrangeTask(ArrangeTaskSubmission submission)
        {
            var arrangeTask = _learningObjectRepository.GetArrangeTask(submission.ArrangeTaskId);
            var evaluations = arrangeTask.EvaluateSubmission(submission.Containers);

            submission.IsCorrect = evaluations.Select(e => e.SubmissionWasCorrect).All(c => c);
            _submissionRepository.SaveArrangeTaskSubmission(submission);

            return evaluations;
        }
    }
}
