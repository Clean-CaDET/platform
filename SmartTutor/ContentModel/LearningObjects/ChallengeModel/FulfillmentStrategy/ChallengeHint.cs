using System.ComponentModel.DataAnnotations;

namespace SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy
{
    public class ChallengeHint
    {
        [Key] public int Id { get; set; }
        public string Content { get; set; }
        public int LearningObjectSummaryId { get; set; }

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ChallengeHint);
        }

        public bool Equals(ChallengeHint hint)
        {
            return Id.Equals(hint.Id);
        }
    }
}
