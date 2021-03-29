using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy
{
    public abstract class ChallengeFulfillmentStrategy
    {
        public abstract bool CheckChallengeFulfillment(List<CaDETClass> submittetClasses, Challenge challenge);
    }
}
