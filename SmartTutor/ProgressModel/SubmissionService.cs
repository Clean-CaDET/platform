using System;
using SmartTutor.ContentModel.LearningObjects.ArrangeTasks;
using SmartTutor.ContentModel.LearningObjects.Challenges;
using SmartTutor.ContentModel.LearningObjects.Questions;
using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.ProgressModel.Submissions;
using SmartTutor.ProgressModel.Submissions.Repository;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.Lectures.Repository;
using SmartTutor.LearnerModel.Learners.Repository;

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

        public ChallengeEvaluation EvaluateChallenge(ChallengeSubmission submission, int learnerId)
        {
            Challenge challenge = _learningObjectRepository.GetChallenge(submission.ChallengeId);
            if (!IsLearningObjectInLearnersCourses(challenge, learnerId))
            {
                throw new BadHttpRequestException("Learner is not in this challenges course!");
            }
            if (challenge == null) return null;

            var evaluation = challenge.CheckChallengeFulfillment(submission.SourceCode);

            if (evaluation.ChallengeCompleted) submission.MarkCorrect();
            _submissionRepository.SaveChallengeSubmission(submission);

            //TODO: Tie in with Instructor and handle learnerId to get suitable LO for LO summaries.
            evaluation.ApplicableLOs =
                _learningObjectRepository.GetFirstLearningObjectsForSummaries(
                    evaluation.ApplicableHints.GetDistinctLearningObjectSummaries());
            evaluation.SolutionLO = _learningObjectRepository.GetLearningObjectForSummary(challenge.Solution.Id);

            return evaluation;
        }

        public List<AnswerEvaluation> EvaluateAnswers(QuestionSubmission submission, int learnerId)
        {
            var question = _learningObjectRepository.GetQuestion(submission.QuestionId);
            if (!IsLearningObjectInLearnersCourses(question, learnerId))
            {
                throw new BadHttpRequestException("Learner is not in this questions course!");
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
            var courseIds = GetCoursesIdsByLOsId(learningObject);
            var learner = _learnerRepository.GetById(learnerId);

            return learner.CourseEnrollments.Any(learnerCourseEnrollment => courseIds.Contains(learnerCourseEnrollment.CourseId));
        }

        private HashSet<int> GetCoursesIdsByLOsId(LearningObject learningObject)
        {
            var learningObjectSummary =
                _learningObjectRepository.GetLearningObjectSummary(learningObject.LearningObjectSummaryId);
            var knowledgeNodes = _lectureRepository.GetKnowledgeNodesBySummary(learningObjectSummary.Id);
            var courseIds = new HashSet<int>();

            foreach (var lecture in knowledgeNodes.Select(knowledgeNode => _lectureRepository.GetLecture(knowledgeNode.LectureId)))
            {
                courseIds.Add(lecture.CourseId);
            }

            return courseIds;
        }
    }
}