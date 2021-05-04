using SmartTutor.ContentModel.LearningObjects.ArrangeTasks;
using SmartTutor.ContentModel.LearningObjects.Challenges;
using SmartTutor.ContentModel.LearningObjects.Questions;
using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.ProgressModel.Submissions;
using SmartTutor.ProgressModel.Submissions.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using static System.Environment;

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

            SaveFunctionalSubmissionInFile(submission);
            CreateChallengeTestSuiteForLearner(challenge, submission.LearnerId);

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

        public List<AnswerEvaluation> EvaluateAnswers(QuestionSubmission submission)
        {
            var question = _learningObjectRepository.GetQuestion(submission.QuestionId);
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

        private void SaveFunctionalSubmissionInFile(ChallengeSubmission submission)
        {
            string learnerDirPath = GetFolderPath(SpecialFolder.Desktop) + @"\SubmittedCode\" + submission.LearnerId;
            if (!Directory.Exists(learnerDirPath)) Directory.CreateDirectory(learnerDirPath);
            foreach (var line in submission.SourceCode)
                File.WriteAllText(learnerDirPath + "\\Challenge" + submission.ChallengeId + ".cs", line);
        }

        private void CreateChallengeTestSuiteForLearner(Challenge challenge, int learnerId)
        {
            string directoryPath = GetFolderPath(SpecialFolder.Desktop) + @"\" + learnerId + "\\Challenge" + challenge.Id + "TestSuite";
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

            string challengeTestSuitePath = ConfigurationManager.AppSettings["masterTestSuitePath"] + challenge.TestSuiteLocation;
            foreach (FileInfo file in new DirectoryInfo(challengeTestSuitePath).GetFiles("*.cs"))
            {
                string filePath = directoryPath + "\\" + file.Name;
                if (!File.Exists(filePath)) File.Copy(file.FullName, filePath);
            }
        }
    }
}
