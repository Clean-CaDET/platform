using System;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using RepositoryCompiler.Controllers;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;
using SmartTutor.ContentModel.LearningObjects.Repository;
using System.Collections.Generic;

namespace SmartTutor.ContentModel
{
    public class ChallengeService : IChallengeService
    {
        private readonly ILearningObjectRepository _learningObjectRepository;

        public ChallengeService(ILearningObjectRepository learningObjectRepository)
        {
            _learningObjectRepository = learningObjectRepository;
        }

        public ChallengeEvaluation EvaluateSubmission(string[] sourceCode, int challengeId)
        {
            //TODO: Exceptions for bad CaDETClass - BadRequest, No Challenge - NotFound etc. (reexamine best practices)
            List<CaDETClass> solutionAttempt = GetClassesFromSubmittedChallenge(sourceCode);
            if (solutionAttempt == null || solutionAttempt.Count == 0) throw new InvalidOperationException("Invalid submission.");
            Challenge challenge = _learningObjectRepository.GetChallenge(challengeId);
            if (challenge == null) return null;

            var evaluation = challenge.CheckChallengeFulfillment(solutionAttempt);
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
