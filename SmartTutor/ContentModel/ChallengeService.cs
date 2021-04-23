using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using RepositoryCompiler.Controllers;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel;
using SmartTutor.ContentModel.LearningObjects.Repository;
using SmartTutor.ProgressModel.Repository;
using SmartTutor.ProgressModel.Submissions;
using System;
using System.Collections.Generic;

namespace SmartTutor.ContentModel
{
    public class ChallengeService : IChallengeService
    {
        private readonly ILearningObjectRepository _learningObjectRepository;
        private readonly ILearnerRepository _learnerRepository;

        public ChallengeService(ILearningObjectRepository learningObjectRepository, ILearnerRepository learnerRepository)
        {
            _learningObjectRepository = learningObjectRepository;
            _learnerRepository = learnerRepository;
        }

        public ChallengeEvaluation EvaluateSubmission(string[] sourceCode, int challengeId, string traineeId)
        {
            List<CaDETClass> solutionAttempt = GetClassesFromSubmittedChallenge(sourceCode);
            if (solutionAttempt == null || solutionAttempt.Count == 0) throw new InvalidOperationException("Invalid submission.");

            Challenge challenge = _learningObjectRepository.GetChallenge(challengeId);
            if (challenge == null) return null;

            var evaluation = challenge.CheckChallengeFulfillment(solutionAttempt);
            //TODO: Refactor - this is a dirty fix for the purposes of the test.
            _learnerRepository.SaveChallengeSubmission(new ChallengeSubmission
            {
                ChallengeId = challengeId,
                SubmittedCode = sourceCode,
                LearnerId = traineeId,
                IsCorrect = evaluation.ChallengeCompleted
            });

            //TODO: Tie in with progress model and handle traineeId to get suitable LO for LO summaries.
            evaluation.ApplicableLOs =
                _learningObjectRepository.GetFirstLearningObjectsForSummaries(
                    evaluation.ApplicableHints.GetDistinctLearningObjectSummaries());
            evaluation.SolutionLO = _learningObjectRepository.GetLearningObjectForSummary(challenge.Solution.Id);
            
            return evaluation;
        }

        private List<CaDETClass> GetClassesFromSubmittedChallenge(string[] sourceCode)
        {
            //TODO: Adhere to DIP for CodeModelFactory/CodeRepoService (extract interface and add DI in startup)
            return new CodeRepositoryService().BuildClassesModel(sourceCode);
        }
    }
}
