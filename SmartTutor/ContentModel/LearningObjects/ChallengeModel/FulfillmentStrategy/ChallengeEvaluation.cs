using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy
{
    public class ChallengeEvaluation
    {
        public bool ChallengeCompleted { get; internal set; }
        public List<ChallengeHint> ApplicableHints { get; internal set; }
    }
}
