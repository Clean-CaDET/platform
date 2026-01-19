namespace SmartTutor.ContentModel.LearningObjects.Challenges
{
    public class ChallengeHint
    {
        public int Id { get; private set; }
        public string Content { get; private set; }
        public int? LearningObjectSummaryId { get; private set; }

        private ChallengeHint() {}
        public ChallengeHint(int id): this()
        {
            Id = id;
        }

        public ChallengeHint(int id, string content): this(id)
        {
            Content = content;
        }

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


