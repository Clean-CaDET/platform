using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.ProgressModel
{
    public class Trainee
    {
        [Key] public int Id { get; set; }
        public List<NodeProgress> Progress { get; set; }

        // TODO: Entity framework cannot map dictionaries, requires refactoring
        public int VisualScore { get; set; }
        public int AuralScore { get; set; }
        public int ReadWriteScore { get; set; }
        public int KinaestheticScore { get; set; }

        public Dictionary<LearningPreference, int> LearningPreferenceScore()
        {
            return new Dictionary<LearningPreference, int>
            {
                {LearningPreference.Aural, AuralScore},
                {LearningPreference.Kinaesthetic, KinaestheticScore},
                {LearningPreference.Visual, VisualScore},
                {LearningPreference.ReadWrite, ReadWriteScore}
            };
        }
    }
}