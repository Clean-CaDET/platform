using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy
{
    public abstract class ChallengeFulfillmentStrategy
    {
        [Key] public int Id { get; set; }
        public abstract bool CheckChallengeFulfillment(List<CaDETClass> solutionAttempt);
    }
}
