using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.MetricRules;
using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel
{
    public class Challenge : LearningObject
    {
        public string Url { get; internal set; }
        public List<CaDETClass> ResolvedClasses { get; internal set; }
        public List<MetricRangeRule> ClassMetricRules { get; internal set; }
        public List<MetricRangeRule> MethodMetricRules { get; internal set; }
        public ChallengeFulfillmentStrategy FulfillmentStrategy { get; internal set; }

        public bool CheckSubmittedChallengeCompletion(List<CaDETClass> submittetClasses, Challenge challenge)
        {
            return FulfillmentStrategy.CheckChallengeFulfillment(submittetClasses, challenge);
        }
    }
}