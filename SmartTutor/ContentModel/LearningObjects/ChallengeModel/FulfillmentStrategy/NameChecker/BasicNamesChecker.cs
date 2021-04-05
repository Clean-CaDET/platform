using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy.NameChecker
{
    [Table("BasicNamesChecker")]
    public class BasicNamesChecker : ChallengeFulfillmentStrategy
    {
        public List<string> BannedWords { get; private set; }
        public List<string> RequiredWords { get; private set; }

        public BasicNamesChecker() { }

        public BasicNamesChecker(List<string> bannedWords, List<string> requiredWords)
        {
            BannedWords = bannedWords;
            RequiredWords = requiredWords;
            ChallengeHints = GetChallengeHints();
        }

        public override ChallengeEvaluation CheckChallengeFulfillment(List<CaDETClass> solutionAttempt)
        {
            List<ChallengeHint> challengeHints = GetHintsForSolutionAttempt(solutionAttempt);
            if (!challengeHints.Any()) return new ChallengeEvaluation(true, ChallengeHints);
            return new ChallengeEvaluation(false, challengeHints);
        }

        private List<ChallengeHint> GetHintsForSolutionAttempt(List<CaDETClass> solutionAttempt)
        {
            // TODO: implement
            return new List<ChallengeHint>();
        }

        private List<ChallengeHint> GetChallengeHints()
        {
            // TODO: implement
            return new List<ChallengeHint>();
        }
    }
}
