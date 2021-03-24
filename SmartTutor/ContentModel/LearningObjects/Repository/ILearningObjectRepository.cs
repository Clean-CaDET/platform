using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects.Repository
{
    public interface ILearningObjectRepository
    {
        List<LearningObject> GetLearningObjectsForSummary(int summaryId);
    }
}
