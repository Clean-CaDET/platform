using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy.MetricChecker
{
    [Table("BasicMetricCheckers")]
    public class BasicMetricsChecker : ChallengeFulfillmentStrategy
    {
        public List<MetricRangeRule> ClassMetricRules { get; private set; }
        public List<MetricRangeRule> MethodMetricRules { get; private set; }

        public BasicMetricsChecker() { }

        public BasicMetricsChecker(List<MetricRangeRule> classMetricRules, List<MetricRangeRule> methodMetricRules)
        {
            ClassMetricRules = classMetricRules;
            MethodMetricRules = methodMetricRules;
            ChallengeHints = GetChallengeHints();
        }

        public override ChallengeEvaluation CheckChallengeFulfillment(List<CaDETClass> solutionAttempt)
        {
            List<CaDETMember> submittedMethods = GetMethodsFromClasses(solutionAttempt);
            List<ChallengeHint> challengeHints = GetHintsForSolutionAttempt(solutionAttempt, submittedMethods);
            if (!challengeHints.Any()) return new ChallengeEvaluation(true, ChallengeHints);
            return new ChallengeEvaluation(false, challengeHints);
        }

        private List<CaDETMember> GetMethodsFromClasses(List<CaDETClass> caDETClasses)
        {
            return caDETClasses.SelectMany(c => c.Members.Where(m => m.Type.Equals(CaDETMemberType.Method))).ToList();
        }

        private List<ChallengeHint> GetChallengeHints()
        {
            List<ChallengeHint> challengeHints = new List<ChallengeHint>();
            challengeHints.AddRange(GetHintsForMetricRules(ClassMetricRules));
            challengeHints.AddRange(GetHintsForMetricRules(MethodMetricRules));
            return challengeHints;
        }

        private List<ChallengeHint> GetHintsForMetricRules(List<MetricRangeRule> metricRangeRules)
        {
            List<ChallengeHint> challengeHints = new List<ChallengeHint>();
            foreach (MetricRangeRule metricRangeRule in metricRangeRules)
                if (metricRangeRule.Hint != null) challengeHints.Add(metricRangeRule.Hint);
            return challengeHints;
        }

        private List<ChallengeHint> GetHintsForSolutionAttempt(List<CaDETClass> submittedClasses, List<CaDETMember> submittedMethods)
        {
            List<ChallengeHint> challengeHints = new List<ChallengeHint>();
            challengeHints.AddRange(GetApplicableHintsForIncompleteClass(submittedClasses));
            challengeHints.AddRange(GetApplicableHintsForIncompleteMethod(submittedMethods));
            return challengeHints;
        }

        private List<ChallengeHint> GetApplicableHintsForIncompleteClass(List<CaDETClass> solutionAttempt)
        {
            List<ChallengeHint> challengeHints = new List<ChallengeHint>();
            foreach (CaDETClass caDETClass in solutionAttempt)
            {
                foreach (MetricRangeRule classMetricRule in ClassMetricRules)
                {
                    if (!classMetricRule.MetricMeetsRequirements(caDETClass.Metrics) && classMetricRule.Hint != null)
                        challengeHints.Add(classMetricRule.Hint);
                }
            }
            return challengeHints;
        }

        private List<ChallengeHint> GetApplicableHintsForIncompleteMethod(List<CaDETMember> caDETMethods)
        {
            List<ChallengeHint> challengeHints = new List<ChallengeHint>();
            foreach (CaDETMember caDETMethod in caDETMethods)
            {
                foreach (MetricRangeRule methodMetricRule in MethodMetricRules)
                {
                    if (!methodMetricRule.MetricMeetsRequirements(caDETMethod.Metrics) && methodMetricRule.Hint != null)
                        challengeHints.Add(methodMetricRule.Hint);
                }
            }
            return challengeHints;
        }
    }
}