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

        public ChallengeService() { }

        internal Dictionary<string, double> GetMetricNamesFromClasses(List<CaDETClass> caDETClasses)
        {
            Dictionary<string, double> metrics = new Dictionary<string, double>();
            int classCounter = 0;
            foreach (CaDETClass caDETClass in caDETClasses)
            {
                metrics.Add("LOC " + ++classCounter, 0);
                metrics.Add("LCOM " + classCounter, 0);
                metrics.Add("NMD " + classCounter, 0);
                metrics.Add("NAD " + classCounter, 0);
                metrics.Add("WMC " + classCounter, 0);
                metrics.Add("ATFD " + classCounter, 0);
                metrics.Add("TCC " + classCounter, 0);

                int memberCounter = 0;
                foreach (CaDETMember caDETMember in caDETClass.Members)
                {
                    metrics.Add("CYCLO " + ++memberCounter + " " + classCounter, 0);
                    metrics.Add("LOC " + memberCounter + " " + classCounter, 0);
                    metrics.Add("ELOC " + memberCounter + " " + classCounter, 0);
                    metrics.Add("NOP " + memberCounter + " " + classCounter, 0);
                    metrics.Add("NOLV " + memberCounter + " " + classCounter, 0);
                }
            }
            return metrics;
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
