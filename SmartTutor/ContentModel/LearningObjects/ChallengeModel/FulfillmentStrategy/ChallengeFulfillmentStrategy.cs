using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy
{
    public abstract class ChallengeFulfillmentStrategy
    {
        [Key] public int Id { get; set; }
        public List<ChallengeHint> ChallengeHints { get; internal set; }

        public abstract ChallengeEvaluation CheckChallengeFulfillment(List<CaDETClass> solutionAttempt);
    }
}
