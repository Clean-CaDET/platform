using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy.NameChecker
{
    public class NamingRule
    {
        [Key] public int Id { get; set; }
        public List<string> BannedWords { get; set; }
        public List<string> RequiredWords { get; set; }
        public ChallengeHint Hint { get; set; }

        internal bool NamesMeetRequirements(List<string> allNames)
        {
            return true;
        }
    }
}
