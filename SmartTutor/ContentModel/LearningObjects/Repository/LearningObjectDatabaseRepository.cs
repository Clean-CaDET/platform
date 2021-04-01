using SmartTutor.ContentModel.LearningObjects.ChallengeModel;
using SmartTutor.Database;
using System.Collections.Generic;
using System.Linq;

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
            return _dbContext.LearningObjects.Where(lo => lo.LearningObjectSummaryId == summaryId).ToList();
        }

        public List<LearningObject> GetFirstLearningObjectsForSummaries(List<int> summaries)
        {
            return summaries.SelectMany(GetLearningObjectsForSummary).ToList();
        }

        public Challenge GetChallenge(int challengeId)
        {
            return _dbContext.Challenges.SingleOrDefault(c => c.Id == challengeId);
        }
    }
}