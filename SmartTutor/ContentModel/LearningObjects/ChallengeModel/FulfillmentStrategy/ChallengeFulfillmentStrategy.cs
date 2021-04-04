using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy
{
    public abstract class ChallengeFulfillmentStrategy
    {
        [Key] public int Id { get; private set; }
        public List<ChallengeHint> ChallengeHints { get; internal set; }

        public abstract ChallengeEvaluation CheckChallengeFulfillment(List<CaDETClass> solutionAttempt);
        public abstract List<ChallengeHint> GetHintsForSolutionAttempt(List<CaDETClass> submittedClasses);

        protected List<CaDETMember> GetMethodsFromClasses(List<CaDETClass> caDETClasses)
        {
            return caDETClasses.SelectMany(c => c.Members.Where(m => m.Type.Equals(CaDETMemberType.Method))).ToList();
        }
    }
}
