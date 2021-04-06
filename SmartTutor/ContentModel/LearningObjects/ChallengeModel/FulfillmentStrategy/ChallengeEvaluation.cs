using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy
{
    public class ChallengeEvaluation
    {
        public bool ChallengeCompleted => ApplicableHints.IsEmpty();
        public HintDirectory ApplicableHints { get; }
        public List<LearningObject> ApplicableLOs { get; set; }

        public ChallengeEvaluation()
        {
            ApplicableHints = new HintDirectory();
            ApplicableLOs = new List<LearningObject>();
        }
    }
}
