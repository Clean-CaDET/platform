namespace SmartTutor.ContentModel.LearningObjects
{
    public class LearningObject
    {
        public int Id { get; private set; }
        public int LearningObjectSummaryId { get; private set; }

        protected LearningObject() {}
        public LearningObject(int id, int learningObjectSummaryId)
        {
            Id = id;
            LearningObjectSummaryId = learningObjectSummaryId;
        }
    }
}