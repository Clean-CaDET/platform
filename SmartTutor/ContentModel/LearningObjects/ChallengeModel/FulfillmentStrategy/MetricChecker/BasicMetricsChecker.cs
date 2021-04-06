using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy.MetricChecker
{
    [Table("BasicMetricCheckers")]
    public class BasicMetricsChecker : ChallengeFulfillmentStrategy
    {
        public List<MetricRangeRule> ClassMetricRules { get; set; }
        public List<MetricRangeRule> MethodMetricRules { get; set; }

        public override ChallengeEvaluation CheckChallengeFulfillment(List<CaDETClass> solutionAttempt)
        {
            var challengeHints = GetHintsForSolutionAttempt(solutionAttempt);
            if (!challengeHints.Any()) return new ChallengeEvaluation(true, GetAllHints());
            return new ChallengeEvaluation(false, challengeHints);
        }

        public override List<ChallengeHint> GetAllHints()
        {
            var challengeHints = new List<ChallengeHint>();
            challengeHints.AddRange(ClassMetricRules.Select(c => c.BaseHint));
            challengeHints.AddRange(MethodMetricRules.Select(m => m.BaseHint));
            return challengeHints;
        }

        public List<ChallengeHint> GetHintsForSolutionAttempt(List<CaDETClass> submittedClasses)
        {
            List<ChallengeHint> challengeHints = new List<ChallengeHint>();
            challengeHints.AddRange(GetApplicableHintsForIncompleteClasses(submittedClasses));
            challengeHints.AddRange(GetApplicableHintsForIncompleteMethods(submittedClasses));
            return challengeHints;
        }

        private List<ChallengeHint> GetApplicableHintsForIncompleteClasses(List<CaDETClass> solutionAttempt)
        {
            var challengeHints = new List<ChallengeHint>();
            foreach (var caDETClass in solutionAttempt)
            {
                foreach (var metricRule in ClassMetricRules)
                {
                    var result = metricRule.Evaluate(caDETClass.Metrics);
                    if (result == null) continue;
                    result.ApplicableCodeSnippetId = caDETClass.FullName;
                    challengeHints.Add(result);
                }
            }
            return challengeHints;
        }

        private List<ChallengeHint> GetApplicableHintsForIncompleteMethods(List<CaDETClass> solutionAttempt)
        {
            var challengeHints = new List<ChallengeHint>();
            foreach (var caDETMethod in GetMethodsFromClasses(solutionAttempt))
            {
                foreach (var metricRule in MethodMetricRules)
                {
                    var result = metricRule.Evaluate(caDETMethod.Metrics);
                    if (result == null) continue;
                    result.ApplicableCodeSnippetId = caDETMethod.Signature();
                    challengeHints.Add(result);
                }
            }
            return challengeHints;
        }
    }
}