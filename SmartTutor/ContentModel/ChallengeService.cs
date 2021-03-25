using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using RepositoryCompiler.Controllers;
using SmartTutor.ContentModel.LearningObjects;
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

        public bool CheckSubmittedChallengeCompletion(string[] sourceCode, int challengeId)
        {
            List<CaDETClass> endState = GetChallenge(challengeId).EndState;
            List<CaDETClass> submittetState = GetClassesFromSubmittedChallenge(sourceCode);
            return CompareChallengeStates(endState, submittetState);
        }

        public Challenge GetChallenge(int challengeId)
        {
            return _learningObjectRepository.GetLearningObjectForChallenge(challengeId) as Challenge;
        }

        private List<CaDETClass> GetClassesFromSubmittedChallenge(string[] sourceCode)
        {
            return new CodeRepositoryService().BuildClassesModel(sourceCode);
        }

        private bool CompareChallengeStates(List<CaDETClass> endState, List<CaDETClass> submittetState)
        {
            // TODO: Implement rang for correct state, current is the simplest stage
            if (endState != submittetState)
                return false;

            return true;
        }
    }
}
