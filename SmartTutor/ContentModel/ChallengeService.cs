using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using RepositoryCompiler.Controllers;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.SourceCode;
using SmartTutor.ContentModel.LearningObjects.Repository;
using System;
using System.Collections.Generic;
using SmartTutor.ContentModel.ProgressModel;

namespace SmartTutor.ContentModel
{
    public class ChallengeService : IChallengeService
    {
        private readonly ILearningObjectRepository _learningObjectRepository;
        private readonly ITraineeRepository _traineeRepository;

        public ChallengeService(ILearningObjectRepository learningObjectRepository, ITraineeRepository traineeRepository)
        {
            _learningObjectRepository = learningObjectRepository;
            _traineeRepository = traineeRepository;
        }

        public ChallengeEvaluation EvaluateSubmission(string[] sourceCode, int challengeId, string traineeId)
        {
            //TODO: Exceptions for bad CaDETClass - BadRequest, No Challenge - NotFound etc. (reexamine best practices)
            List<CaDETClass> solutionAttempt = GetClassesFromSubmittedChallenge(sourceCode);
            if (solutionAttempt == null || solutionAttempt.Count == 0) throw new InvalidOperationException("Invalid submission.");

            Challenge challenge = _learningObjectRepository.GetChallenge(challengeId);
            if (challenge == null) return null;

            var evaluation = challenge.CheckChallengeFulfillment(solutionAttempt);
            //TODO: Refactor - this is a dirty fix for the purposes of the test.
            _traineeRepository.SaveChallengeSubmission(new ChallengeSubmission
            {
                ChallengeId = challengeId,
                SubmittedCode = sourceCode,
                TraineeId = traineeId,
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
