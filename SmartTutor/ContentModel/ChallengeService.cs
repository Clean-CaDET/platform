using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using RepositoryCompiler.Controllers;
using SmartTutor.ContentModel.LearningObjects;
using SmartTutor.ContentModel.LearningObjects.Repository;
using System.Collections.Generic;
using System.Linq;

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
            List<CaDETClass> submittetClasses = GetClassesFromSubmittedChallenge(sourceCode);
            return CheckMetricsForChallenge(GetMetricsFromClasses(submittetClasses), GetChallenge(challengeId));
        }

        public Challenge GetChallenge(int challengeId)
        {
            return _learningObjectRepository.GetChallenge(challengeId);
        }

        private List<CaDETClass> GetClassesFromSubmittedChallenge(string[] sourceCode)
        {
            return new CodeRepositoryService().BuildClassesModel(sourceCode);
        }

        private bool CheckMetricsForChallenge(Dictionary<string, double> submittetMetrics, Challenge challenge)
        {
            foreach (string metricName in submittetMetrics.Keys.ToList())
            {
                Dictionary<string, double> resultMetrics = GetMetricsFromClasses(challenge.ResolvedClasses);
                if (resultMetrics[metricName] - challenge.MetricsRange[metricName] <= submittetMetrics[metricName]
                    && submittetMetrics[metricName] <= resultMetrics[metricName] + challenge.MetricsRange[metricName])
                    return true;
            }

            return false;
        }

        // TODO: Developer might make more/less classes or members than there is in endState, change for those cases
        private Dictionary<string, double> GetMetricsFromClasses(List<CaDETClass> caDETClasses)
        {
            Dictionary<string, double> metrics = new Dictionary<string, double>();
            int classCounter = 0;
            foreach (CaDETClass caDETClass in caDETClasses)
            {
                metrics.Add("LOC " + ++classCounter, caDETClass.Metrics.LOC);
                metrics.Add("LCOM " + classCounter, (double)caDETClass.Metrics.LCOM);
                metrics.Add("NMD " + classCounter, caDETClass.Metrics.NMD);
                metrics.Add("NAD " + classCounter, caDETClass.Metrics.NAD);
                metrics.Add("WMC " + classCounter, caDETClass.Metrics.WMC);
                metrics.Add("ATFD " + classCounter, caDETClass.Metrics.ATFD);
                metrics.Add("TCC " + classCounter, (double)caDETClass.Metrics.TCC);

                int memberCounter = 0;
                foreach (CaDETMember caDETMember in caDETClass.Members)
                {
                    metrics.Add("CYCLO " + ++memberCounter + " " + classCounter, caDETMember.Metrics.CYCLO);
                    metrics.Add("LOC " + memberCounter + " " + classCounter, caDETMember.Metrics.LOC);
                    metrics.Add("ELOC " + memberCounter + " " + classCounter, caDETMember.Metrics.ELOC);
                    metrics.Add("NOP " + memberCounter + " " + classCounter, caDETMember.Metrics.NOP);
                    metrics.Add("NOLV " + memberCounter + " " + classCounter, caDETMember.Metrics.NOLV);
                }
            }
            return metrics;
        }
    }
}
