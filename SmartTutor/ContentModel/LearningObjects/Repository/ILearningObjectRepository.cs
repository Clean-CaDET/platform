using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects.Repository
{
    public interface ILearningObjectRepository
    {
        List<LearningObject> GetLearningObjectsForSummary(int summaryId);
        List<LearningObject> GetFirstLearningObjectsForSummaries(List<int> summaries);
        LearningObject GetLearningObjectForChallenge(int challengeId);
    }
}