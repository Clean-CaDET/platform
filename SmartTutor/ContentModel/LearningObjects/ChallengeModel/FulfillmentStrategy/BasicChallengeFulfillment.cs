using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.MetricRules;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy
{
    public class BasicChallengeFulfillment : ChallengeFulfillmentStrategy
    {
        public override bool CheckChallengeFulfillment(List<CaDETClass> submittetClasses, Challenge challenge)
        {
            List<CaDETMember> submittedMethods = GetMethodsFromClasses(submittetClasses);
            return ValidateClassMetricRules(submittetClasses, challenge) && ValidateMethodMetricRules(submittedMethods, challenge);
        }

        private List<CaDETMember> GetMethodsFromClasses(List<CaDETClass> caDETClasses)
        {
            return caDETClasses.SelectMany(c => c.Members.Where(m => m.Type.Equals(CaDETMemberType.Method))).ToList();
        }

        private bool ValidateClassMetricRules(List<CaDETClass> caDETClasses, Challenge challenge)
        {
            foreach (CaDETClass caDETClass in caDETClasses)
                foreach (MetricRangeRule classMetricRule in challenge.ClassMetricRules)
                    if (!classMetricRule.MetricMeetsRequirements(caDETClass.Metrics))
                        return false;
            return true;
        }

        private bool ValidateMethodMetricRules(List<CaDETMember> caDETMethods, Challenge challenge)
        {
            foreach (CaDETMember caDETMethod in caDETMethods)
                foreach (MetricRangeRule methodMetricRule in challenge.MethodMetricRules)
                    if (!methodMetricRule.MetricMeetsRequirements(caDETMethod.Metrics))
                        return false;
            return true;
        }
    }
}