using Microsoft.EntityFrameworkCore;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel;
using SmartTutor.Database;
using System.Collections.Generic;
using System.Linq;
using SmartTutor.ContentModel.LearningObjects.ChallengeModel.FulfillmentStrategy.MetricChecker;

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
                .Include(lo => (lo as Question).PossibleAnswers)
                .Include(lo => (lo as ArrangeTask).Containers)
                .ThenInclude(c => c.Elements);
            return query.ToList();
        }

        public List<LearningObject> GetFirstLearningObjectsForSummaries(List<int> summaries)
        {
            return summaries.Select(summaryId => GetLearningObjectsForSummary(summaryId).First()).ToList();
        }

        public Challenge GetChallenge(int challengeId)
        {
            return _dbContext.Challenges
                .Where(c => c.Id == challengeId)
                .Include(c => c.FulfillmentStrategies)
                    .ThenInclude(s => (s as BasicMetricChecker).ClassMetricRules)
                    .ThenInclude(r => r.Hint)
                .Include(c => c.FulfillmentStrategies)
                    .ThenInclude(s => (s as BasicMetricChecker).MethodMetricRules)
                    .ThenInclude(r => r.Hint)
                .FirstOrDefault();
        }

        public List<QuestionAnswer> GetQuestionAnswers(int questionId)
        {
            return _dbContext.QuestionAnswers.Where(a => a.QuestionId == questionId).ToList();
        }

        public List<ArrangeTaskContainer> GetArrangeTaskContainers(int arrangeTaskId)
        {
            return _dbContext.ArrangeTaskContainers.Where(c => c.ArrangeTaskId == arrangeTaskId)
                .Include(c => c.Elements).ToList();
        }

        public Image GetImageForSummary(int summaryId)
        {
            return _dbContext.Images.FirstOrDefault(learningObj => learningObj.LearningObjectSummaryId == summaryId);
        }

        public Question GetQuestionForSummary(int summaryId)
        {
            return _dbContext.Questions.FirstOrDefault(learningObj => learningObj.LearningObjectSummaryId == summaryId);
        }

        public Text GetTextForSummary(int summaryId)
        {
            return _dbContext.Texts.FirstOrDefault(learningObj => learningObj.LearningObjectSummaryId == summaryId);
        }

        public Video GetVideoForSummary(int summaryId)
        {
            return _dbContext.Videos.FirstOrDefault(learningObj => learningObj.LearningObjectSummaryId == summaryId);
        }

        public Challenge GetChallengeForSummary(int summaryId)
        {
            throw new System.NotImplementedException();
        }
    }
}