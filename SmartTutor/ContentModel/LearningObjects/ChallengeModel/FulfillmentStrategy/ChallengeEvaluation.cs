using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy
{
    public class ChallengeEvaluation
    {
        public bool ChallengeCompleted => ApplicableHints.IsEmpty();
        public HintDirectory ApplicableHints { get; }

        public ChallengeEvaluation()
        {
            ApplicableHints = new HintDirectory();
        }
    }
}
