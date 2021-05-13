using CodeModel.CaDETModel.CodeItems;
using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy
{
    public abstract class ChallengeFulfillmentStrategy
    {
        public int Id { get; private set; }

        public abstract HintDirectory EvaluateSubmission(List<CaDETClass> solutionAttempt);
        public abstract List<ChallengeHint> GetAllHints();
    }
}
