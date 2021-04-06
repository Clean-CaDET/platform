using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel
{
    [Table("Challenges")]
    public class Challenge : LearningObject
    {
        public string Url { get; internal set; }
        public string Description { get; internal set; }
        public List<ChallengeFulfillmentStrategy> FulfillmentStrategies { get; internal set; }

        internal ChallengeEvaluation CheckChallengeFulfillment(List<CaDETClass> solutionAttempt)
        {
            var evaluation = new ChallengeEvaluation();
            foreach (var strategy in FulfillmentStrategies)
            {
                var result = strategy.EvaluateSubmission(solutionAttempt);
                evaluation.ApplicableHints.MergeHints(result);
            }

            return evaluation;
        }

        internal List<ChallengeHint> GetAllChallengeHints()
        {
            return FulfillmentStrategies.SelectMany(s => s.GetAllHints().Where(h => h != null)).ToList();
        }
    }
}