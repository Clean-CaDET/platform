using System.Collections.Generic;
using SmartTutor.ProgressModel.Progress;

namespace SmartTutor.ContentModel.LearningObjects
{
    public class LearningObject
    {
        public int Id { get; private set; }
        public int LearningObjectSummaryId { get; private set; }
        public List<NodeProgress> NodeProgresses { get; set; }

        protected LearningObject() {}
        protected LearningObject(int id, int learningObjectSummaryId)
        {
            Id = id;
            LearningObjectSummaryId = learningObjectSummaryId;
        }
    }
}