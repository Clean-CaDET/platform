using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.MetricHints;
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
            ChallengeEvaluation challengeEvaluation = new ChallengeEvaluation
            {
                ChallengeCompleted = ValidateClassMetricRules(solutionAttempt) && ValidateMethodMetricRules(submittedMethods),
                ApplicableHints = GetHintsForSolutionAttempt(solutionAttempt, submittedMethods)
            };
            return challengeEvaluation;
        }

        private List<CaDETMember> GetMethodsFromClasses(List<CaDETClass> caDETClasses)
        {
            return caDETClasses.SelectMany(c => c.Members.Where(m => m.Type.Equals(CaDETMemberType.Method))).ToList();
        }

        private bool ValidateClassMetricRules(List<CaDETClass> caDETClasses)
        {
            foreach (CaDETClass caDETClass in caDETClasses)
            {
                foreach (MetricRangeRule classMetricRule in ClassMetricRules)
                {
                    if (!classMetricRule.MetricMeetsRequirements(caDETClass.Metrics)) return false;
                }
            }
            return true;
        }

        private bool ValidateMethodMetricRules(List<CaDETMember> caDETMethods)
        {
            foreach (CaDETMember caDETMethod in caDETMethods)
            {
                foreach (MetricRangeRule methodMetricRule in MethodMetricRules)
                {
                    if (!methodMetricRule.MetricMeetsRequirements(caDETMethod.Metrics)) return false;
                }
            }
            return true;
        }

        private List<ChallengeHint> GetChallengeHints()
        {
            List<ChallengeHint> challengeHints = new List<ChallengeHint>();
            challengeHints.AddRange(GetHintsForClassMetricRules(ClassMetricRules));
            challengeHints.AddRange(GetHintsForMethodMetricRules(MethodMetricRules));
            return challengeHints;
        }

        private List<ChallengeHint> GetHintsForClassMetricRules(List<MetricRangeRule> metricRangeRules)
        {
            List<ChallengeHint> challengeHints = new List<ChallengeHint>();
            foreach (MetricRangeRule metricRangeRule in metricRangeRules)
            {
                challengeHints.Add(metricRangeRule.GetHintForIncompleteClass());
            }
            return challengeHints;
        }

        private List<ChallengeHint> GetHintsForMethodMetricRules(List<MetricRangeRule> metricRangeRules)
        {
            List<ChallengeHint> challengeHints = new List<ChallengeHint>();
            foreach (MetricRangeRule metricRangeRule in metricRangeRules)
            {
                challengeHints.Add(metricRangeRule.GetHintForIncompleteMethod());
            }
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
                    if (!classMetricRule.MetricMeetsRequirements(caDETClass.Metrics))
                        challengeHints.Add(classMetricRule.GetApplicableHintForIncompleteClass(caDETClass));
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
                    if (!methodMetricRule.MetricMeetsRequirements(caDETMethod.Metrics))
                        challengeHints.Add(methodMetricRule.GetApplicableHintForIncompleteMethod(caDETMethod));
                }
            }
            return challengeHints;
        }
    }
}