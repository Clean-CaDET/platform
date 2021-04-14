using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel
{
    public class ChallengeEvaluation
    {
        [Key] public int Id { get; set; }
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
