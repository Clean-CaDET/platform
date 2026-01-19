namespace SmartTutor.ContentModel.LearningObjects
{
    public class Image : LearningObject
    {
        public string Url { get; private set; }
        public string Caption { get; private set; }

        public Image(int id, int learningObjectSummaryId, string url, string caption) : base(id, learningObjectSummaryId)
        {
            Url = url;
            Caption = caption;
        }
    }
}