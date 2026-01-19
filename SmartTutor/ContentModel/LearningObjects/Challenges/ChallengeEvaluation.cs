using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects.Challenges
{
    public class ChallengeEvaluation
    {
        public int ChallengeId { get; private set; }
        public bool ChallengeCompleted { get; internal set; }
        public HintDirectory ApplicableHints { get; }
        public List<LearningObject> ApplicableLOs { get; internal set; }
        public LearningObject SolutionLO { get; internal set; }

        public ChallengeEvaluation(int challengeId)
        {
            ChallengeId = challengeId;
            ApplicableHints = new HintDirectory();
            ApplicableLOs = new List<LearningObject>();
        }
    }
}
