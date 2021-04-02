﻿using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
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

        public ChallengeEvaluation CheckChallengeCompletion(string[] sourceCode, int challengeId)
        {
            List<CaDETClass> solutionAttempt = GetClassesFromSubmittedChallenge(sourceCode);
            Challenge challenge = GetChallenge(challengeId);
            return challenge.CheckChallengeFulfillment(solutionAttempt);
        }

        public Challenge GetChallenge(int challengeId)
        {
            return _learningObjectRepository.GetChallenge(challengeId);
        }

        private List<CaDETClass> GetClassesFromSubmittedChallenge(string[] sourceCode)
        {
            return new CodeRepositoryService().BuildClassesModel(sourceCode);
        }
    }
}
