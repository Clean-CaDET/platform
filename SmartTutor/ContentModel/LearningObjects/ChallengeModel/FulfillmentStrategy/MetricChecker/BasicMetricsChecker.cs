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

        private List<MetricHint> _metricHints;

        public BasicMetricsChecker() { }

        public BasicMetricsChecker(List<MetricRangeRule> classMetricRules, List<MetricRangeRule> methodMetricRules, List<MetricHint> metricHints)
        {
            ClassMetricRules = classMetricRules;
            MethodMetricRules = methodMetricRules;
            _metricHints = metricHints;
        }

        //TODO: return object (with hints), not just bool
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
            int flag = 0;
            _metricHints = new List<MetricHint>();
            foreach (CaDETClass caDETClass in caDETClasses)
            {
                foreach (MetricRangeRule classMetricRule in ClassMetricRules)
                {
                    if (!classMetricRule.MetricMeetsRequirements(caDETClass.Metrics))
                    {
                        flag++;
                        AddHintForUnfulfilledMetricRule(classMetricRule);
                    }
                }
            }
            return (flag == 0);
        }

        private bool ValidateMethodMetricRules(List<CaDETMember> caDETMethods)
        {
            int flag = 0;
            foreach (CaDETMember caDETMethod in caDETMethods)
            {
                foreach (MetricRangeRule methodMetricRule in MethodMetricRules)
                {
                    if (!methodMetricRule.MetricMeetsRequirements(caDETMethod.Metrics))
                    {
                        flag++;
                        AddHintForUnfulfilledMetricRule(methodMetricRule);
                    }
                }
            }
            return (flag == 0);
        }

        private void AddHintForUnfulfilledMetricRule(MetricRangeRule metricRule)
        {
            _metricHints.Add(new MetricHint
            {
                Content = "Metric rule " + metricRule.MetricName + " should be between " + metricRule.FromValue + " and " + metricRule.ToValue + "."
            });
        }
    }
}