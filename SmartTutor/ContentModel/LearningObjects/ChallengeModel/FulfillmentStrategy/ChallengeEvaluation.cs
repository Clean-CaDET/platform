using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy
{
    public class ChallengeEvaluation
    {
        public bool ChallengeCompleted { get; private set; }
        public List<ChallengeHint> ApplicableHints { get; private set; }

        public ChallengeEvaluation(bool challengeCompleted, List<ChallengeHint> applicableHints)
        {
            ChallengeCompleted = challengeCompleted;
            ApplicableHints = applicableHints;
        }
    }
}
