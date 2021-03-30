using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.MetricRules;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy
{
    public class BasicChallengeFulfillment : ChallengeFulfillmentStrategy
    {
        private readonly List<MetricRangeRule> _classMetricRules;
        private readonly List<MetricRangeRule> _methodMetricRules;

        public BasicChallengeFulfillment(List<MetricRangeRule> classMetricRules, List<MetricRangeRule> methodMetricRules)
        {
            _classMetricRules = classMetricRules;
            _methodMetricRules = methodMetricRules;
        }

        public override bool CheckChallengeFulfillment(List<CaDETClass> submittetClasses)
        {
            List<CaDETMember> submittedMethods = GetMethodsFromClasses(submittetClasses);
            return ValidateClassMetricRules(submittetClasses) && ValidateMethodMetricRules(submittedMethods);
        }

        private List<CaDETMember> GetMethodsFromClasses(List<CaDETClass> caDETClasses)
        {
            return caDETClasses.SelectMany(c => c.Members.Where(m => m.Type.Equals(CaDETMemberType.Method))).ToList();
        }

        private bool ValidateClassMetricRules(List<CaDETClass> caDETClasses)
        {
            foreach (CaDETClass caDETClass in caDETClasses)
                foreach (MetricRangeRule classMetricRule in _classMetricRules)
                    if (!classMetricRule.MetricMeetsRequirements(caDETClass.Metrics))
                        return false;
            return true;
        }

        private bool ValidateMethodMetricRules(List<CaDETMember> caDETMethods)
        {
            foreach (CaDETMember caDETMethod in caDETMethods)
                foreach (MetricRangeRule methodMetricRule in _methodMetricRules)
                    if (!methodMetricRule.MetricMeetsRequirements(caDETMethod.Metrics))
                        return false;
            return true;
        }
    }
}