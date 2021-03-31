using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy.MetricChecker
{
    [Table("BasicMetricCheckers")]
    public class BasicMetricsChecker : ChallengeFulfillmentStrategy
    {
        public List<MetricRangeRule> ClassMetricRules { get; set; }
        public List<MetricRangeRule> MethodMetricRules { get; set; }

        public BasicMetricsChecker() {}

        public BasicMetricsChecker(List<MetricRangeRule> classMetricRules, List<MetricRangeRule> methodMetricRules)
        {
            ClassMetricRules = classMetricRules;
            MethodMetricRules = methodMetricRules;
        }

        public override bool CheckChallengeFulfillment(List<CaDETClass> solutionAttempt)
        {
            List<CaDETMember> submittedMethods = GetMethodsFromClasses(solutionAttempt);
            return ValidateClassMetricRules(solutionAttempt) && ValidateMethodMetricRules(submittedMethods);
        }

        private List<CaDETMember> GetMethodsFromClasses(List<CaDETClass> caDETClasses)
        {
            return caDETClasses.SelectMany(c => c.Members.Where(m => m.Type.Equals(CaDETMemberType.Method))).ToList();
        }

        private bool ValidateClassMetricRules(List<CaDETClass> caDETClasses)
        {
            foreach (CaDETClass caDETClass in caDETClasses)
                foreach (MetricRangeRule classMetricRule in ClassMetricRules)
                    if (!classMetricRule.MetricMeetsRequirements(caDETClass.Metrics))
                        return false;
            return true;
        }

        private bool ValidateMethodMetricRules(List<CaDETMember> caDETMethods)
        {
            foreach (CaDETMember caDETMethod in caDETMethods)
                foreach (MetricRangeRule methodMetricRule in MethodMetricRules)
                    if (!methodMetricRule.MetricMeetsRequirements(caDETMethod.Metrics))
                        return false;
            return true;
        }
    }
}