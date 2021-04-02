using Microsoft.EntityFrameworkCore;
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
            var query = _dbContext.LearningObjects
                .Where(lo => lo.LearningObjectSummaryId == summaryId)
                // The Include below enables retrieval of associations related to derived types.
                .Include(lo => (lo as Question).PossibleAnswers);
            return query.ToList();
        }

        public List<LearningObject> GetFirstLearningObjectsForSummaries(List<int> summaries)
        {
            return summaries.Select(summaryId => GetLearningObjectsForSummary(summaryId).First()).ToList();
        }

        public Challenge GetChallenge(int challengeId)
        {
            return _dbContext.Challenges.SingleOrDefault(c => c.Id == challengeId);
        }

        public List<QuestionAnswer> GetQuestionAnswers(int questionId)
        {
            return _dbContext.QuestionAnswers.Where(a => a.QuestionId == questionId).ToList();
        }
    }
}