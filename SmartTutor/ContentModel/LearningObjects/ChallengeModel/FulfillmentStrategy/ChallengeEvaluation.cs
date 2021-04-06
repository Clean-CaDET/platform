using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy
{
    public class ChallengeEvaluation
    {
        public bool ChallengeCompleted { get; internal set; }
        public List<ChallengeHint> ApplicableHints { get; }

        public ChallengeEvaluation()
        {
            ChallengeCompleted = true;
            ApplicableHints = new List<ChallengeHint>();
        }

        public ChallengeEvaluation(bool challengeCompleted, List<ChallengeHint> applicableHints)
        {
            ChallengeCompleted = challengeCompleted;
            ApplicableHints = applicableHints;
        }
    }
}
