using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;
using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel
{
    public class Challenge : LearningObject
    {
        public string Url { get; internal set; }
        public List<CaDETClass> ResolvedClasses { get; internal set; }
        public ChallengeFulfillmentStrategy FulfillmentStrategy { get; internal set; }

        public bool CheckChallengeFulfillment(List<CaDETClass> submittetClasses)
        {
            return FulfillmentStrategy.CheckChallengeFulfillment(submittetClasses);
        }
    }
}