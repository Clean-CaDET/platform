using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel
{
    [Table("Challenges")]
    public class Challenge : LearningObject
    {
        public string Url { get; internal set; }
        public ChallengeFulfillmentStrategy FulfillmentStrategy { get; internal set; }

        public bool CheckChallengeFulfillment(List<CaDETClass> solutionAttempt)
        {
            return FulfillmentStrategy.CheckChallengeFulfillment(solutionAttempt);
        }
    }
}