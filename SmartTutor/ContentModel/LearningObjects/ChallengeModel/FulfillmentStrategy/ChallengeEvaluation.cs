using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy
{
    public class ChallengeEvaluation
    {
        public int ChallengeId { get; set; }
        public bool ChallengeCompleted { get; set; }
        public HintDirectory ApplicableHints { get; }
        public List<LearningObject> ApplicableLOs { get; set; }
        public LearningObject SolutionLO { get; set; }

        public ChallengeEvaluation()
        {
            ApplicableHints = new HintDirectory();
            ApplicableLOs = new List<LearningObject>();
        }
    }
}
