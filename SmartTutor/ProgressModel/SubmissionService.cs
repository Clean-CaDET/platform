using SmartTutor.ContentModel.LearningObjects.ArrangeTasks;
using SmartTutor.ContentModel.LearningObjects.Challenges;
using SmartTutor.ContentModel.LearningObjects.Challenges.FunctionalityTester;
using SmartTutor.ContentModel.LearningObjects.Questions;
using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.ProgressModel.Submissions;
using SmartTutor.ProgressModel.Submissions.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.Lectures.Repository;
using SmartTutor.LearnerModel.Learners.Repository;
using SmartTutor.ProgressModel.Exceptions;

namespace SmartTutor.ProgressModel
{
    public class SubmissionService : ISubmissionService
    {
        private readonly ILearningObjectRepository _learningObjectRepository;
        private readonly ISubmissionRepository _submissionRepository;
        private readonly ILearnerRepository _learnerRepository;
        private readonly ILectureRepository _lectureRepository;

        public SubmissionService(ILearningObjectRepository learningObjectRepository,
            ISubmissionRepository submissionRepository, ILearnerRepository learnerRepository,
            ILectureRepository lectureRepository)
        {
            _learningObjectRepository = learningObjectRepository;
            _submissionRepository = submissionRepository;
            _learnerRepository = learnerRepository;
            _lectureRepository = lectureRepository;
        }

        public ChallengeEvaluation EvaluateChallenge(ChallengeSubmission submission)
        {
            Challenge challenge = _learningObjectRepository.GetChallenge(submission.ChallengeId);
            if (!IsLearningObjectInLearnersCourses(challenge, submission.LearnerId))
            {
                throw new LearnerNotEnrolledInCourse(submission.LearnerId);
            }

            if (challenge == null) return null;

            //var tester = new WorkspaceFunctionalityTester(_submissionRepository.GetWorkspacePath(submission.LearnerId));
            var evaluation = challenge.CheckChallengeFulfillment(submission.SourceCode, null);

            if (evaluation.ChallengeCompleted) submission.MarkCorrect();
            _submissionRepository.SaveChallengeSubmission(submission);

            //TODO: Tie in with Instructor and handle learnerId to get suitable LO for LO summaries.
            evaluation.ApplicableLOs =
                _learningObjectRepository.GetFirstLearningObjectsForSummaries(
                    evaluation.ApplicableHints.GetDistinctLearningObjectSummaries());
            evaluation.SolutionLO = _learningObjectRepository.GetLearningObjectForSummary(challenge.Solution.Id);

            return evaluation;
        }

        public List<AnswerEvaluation> EvaluateAnswers(QuestionSubmission submission)
        {
            var question = _learningObjectRepository.GetQuestion(submission.QuestionId);
            if (!IsLearningObjectInLearnersCourses(question, submission.LearnerId))
            {
                throw new LearnerNotEnrolledInCourse(submission.LearnerId);
            }

            var evaluations = question.EvaluateAnswers(submission.SubmittedAnswerIds);

            if (evaluations.Select(a => a.SubmissionWasCorrect).All(c => c)) submission.MarkCorrect();
            _submissionRepository.SaveQuestionSubmission(submission);

            return evaluations;
        }

        public List<ArrangeTaskContainerEvaluation> EvaluateArrangeTask(ArrangeTaskSubmission submission)
        {
            var arrangeTask = _learningObjectRepository.GetArrangeTask(submission.ArrangeTaskId);
            var evaluations = arrangeTask.EvaluateSubmission(submission.Containers);
            if (evaluations == null) throw new InvalidOperationException("Invalid submission of arrange task.");

            if (evaluations.Select(e => e.SubmissionWasCorrect).All(c => c)) submission.MarkCorrect();
            _submissionRepository.SaveArrangeTaskSubmission(submission);

            return evaluations;
        }

        private bool IsLearningObjectInLearnersCourses(LearningObject learningObject, int learnerId)
        {
            var courseId = _lectureRepository.GetCourseIdByLOId(learningObject.LearningObjectSummaryId);
            var learner = _learnerRepository.GetById(learnerId);
            return learner.CourseEnrollments.Any(learnerCourseEnrollment =>
                learnerCourseEnrollment.CourseId == courseId);
        }
    }
}