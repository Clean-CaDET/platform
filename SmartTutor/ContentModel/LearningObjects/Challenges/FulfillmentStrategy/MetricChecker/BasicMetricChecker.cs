using CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy.MetricChecker
{
    public class BasicMetricChecker : ChallengeFulfillmentStrategy
    {
        public List<MetricRangeRule> ClassMetricRules { get; private set; }
        public List<MetricRangeRule> MethodMetricRules { get; private set; }

        private BasicMetricChecker() {}
        public BasicMetricChecker(List<MetricRangeRule> classMetricRules, List<MetricRangeRule> methodMetricRules) : this()
        {
            ClassMetricRules = classMetricRules;
            MethodMetricRules = methodMetricRules;
        }

        public override HintDirectory EvaluateSubmission(List<CaDETClass> solutionAttempt)
        {
            var challengeHints = GetApplicableHintsForIncompleteClasses(solutionAttempt);
            challengeHints.MergeHints(GetApplicableHintsForIncompleteMethods(solutionAttempt));
            return challengeHints;
        }

        public override List<ChallengeHint> GetAllHints()
        {
            var challengeHints = new List<ChallengeHint>();
            challengeHints.AddRange(ClassMetricRules.Select(c => c.Hint));
            challengeHints.AddRange(MethodMetricRules.Select(m => m.Hint));
            return challengeHints;
        }

        private HintDirectory GetApplicableHintsForIncompleteClasses(List<CaDETClass> solutionAttempt)
        {
            var challengeHints = new HintDirectory();
            foreach (var caDETClass in solutionAttempt)
            {
                foreach (var metricRule in ClassMetricRules)
                {
                    var result = metricRule.Evaluate(caDETClass.Metrics);
                    if (result == null) continue;
                    challengeHints.AddHint(caDETClass.FullName, result);
                }
            }
            return challengeHints;
        }

        private HintDirectory GetApplicableHintsForIncompleteMethods(List<CaDETClass> solutionAttempt)
        {
            var challengeHints = new HintDirectory();
            foreach (var caDETMethod in GetMethodsFromClasses(solutionAttempt))
            {
                foreach (var metricRule in MethodMetricRules)
                {
                    var result = metricRule.Evaluate(caDETMethod.Metrics);
                    if (result == null) continue;
                    challengeHints.AddHint(caDETMethod.Signature(), result);
                }
            }
            return challengeHints;
        }

        private List<CaDETMember> GetMethodsFromClasses(List<CaDETClass> classes)
        {
            return classes.SelectMany(c => c.Members.Where(m => m.Type.Equals(CaDETMemberType.Method))).ToList();
        }
    }
}