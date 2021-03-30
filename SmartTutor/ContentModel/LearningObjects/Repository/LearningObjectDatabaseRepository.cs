using SmartTutor.Database;
using System.Collections.Generic;

namespace SmartTutor.ContentModel.LearningObjects.Repository
{
    public class LearningObjectDatabaseRepository : ILearningObjectRepository
    {
        private readonly SmartTutorContext _dbContext;

        public LearningObjectDatabaseRepository(SmartTutorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<LearningObject> GetLearningObjectsForSummary(int summaryId)
        {
            throw new System.NotImplementedException();
        }

        public List<LearningObject> GetFirstLearningObjectsForSummaries(List<int> summaries)
        {
            throw new System.NotImplementedException();
        }

        public Challenge GetChallenge(int challengeId)
        {
            throw new System.NotImplementedException();
        }
    }
}