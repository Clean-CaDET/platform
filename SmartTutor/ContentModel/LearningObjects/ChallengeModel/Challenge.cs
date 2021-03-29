using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.MetricRules;
using System.Collections.Generic;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel
{
    public class Challenge : LearningObject
    {
        public string Url { get; internal set; }
        public List<CaDETClass> ResolvedClasses { get; internal set; }
        public List<MetricRangeRule> ClassMetricRules { get; internal set; }
        public List<MetricRangeRule> MethodMetricRules { get; internal set; }
        public ChallengeFulfillmentStrategy FulfillmentStrategy { get; internal set; }

        // TODO: This is basic strategy, implement strategy pattern interface 
        public bool CheckSubmittedChallengeCompletion(List<CaDETClass> submittetClasses)
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