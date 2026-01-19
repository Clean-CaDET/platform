using Microsoft.EntityFrameworkCore;
using SmartTutor.ContentModel.LearningObjects.ArrangeTasks;
using SmartTutor.ContentModel.LearningObjects.Challenges;
using SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy.MetricChecker;
using SmartTutor.ContentModel.LearningObjects.Challenges.FulfillmentStrategy.NameChecker;
using SmartTutor.ContentModel.LearningObjects.Questions;
using SmartTutor.Database;
using System.Collections.Generic;
using System.Linq;
using SmartTutor.ContentModel.Lectures;

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
                .Include(c => c.Solution)
                .Include(c => c.FulfillmentStrategies)
                    .ThenInclude(s => (s as BasicMetricChecker).ClassMetricRules)
                    .ThenInclude(r => r.Hint)
                .Include(c => c.FulfillmentStrategies)
                    .ThenInclude(s => (s as BasicMetricChecker).MethodMetricRules)
                    .ThenInclude(r => r.Hint)
                .Include(c => c.FulfillmentStrategies)
                    .ThenInclude(s => (s as BasicNameChecker).Hint)
                .FirstOrDefault();
        }

        public Question GetQuestion(int questionId)
        {
            return _dbContext.Questions.Where(q => q.Id == questionId)
                .Include(q => q.PossibleAnswers).FirstOrDefault();
        }

        public ArrangeTask GetArrangeTask(int arrangeTaskId)
        {
            return _dbContext.ArrangeTasks.Where(t => t.Id == arrangeTaskId)
                .Include(t => t.Containers)
                .ThenInclude(c => c.Elements).FirstOrDefault();
        }

        public Image GetImageForSummary(int summaryId)
        {
            return _dbContext.Images.FirstOrDefault(lo => lo.LearningObjectSummaryId == summaryId);
        }

        public LearningObject GetInteractiveLOForSummary(int summaryId)
        {
            var interactiveLearningObjects = new List<LearningObject>();
            interactiveLearningObjects.AddRange(_dbContext.ArrangeTasks);
            interactiveLearningObjects.AddRange(_dbContext.Questions);
            return interactiveLearningObjects.FirstOrDefault(lo => lo.LearningObjectSummaryId == summaryId);
        }

        public Text GetTextForSummary(int summaryId)
        {
            return _dbContext.Texts.FirstOrDefault(lo => lo.LearningObjectSummaryId == summaryId);
        }

        public Video GetVideoForSummary(int summaryId)
        {
            return _dbContext.Videos.FirstOrDefault(lo => lo.LearningObjectSummaryId == summaryId);
        }

        public LearningObject GetLearningObjectForSummary(int summaryId)
        {
            return _dbContext.LearningObjects.FirstOrDefault(lo => lo.LearningObjectSummaryId == summaryId);
        }

        public LearningObjectSummary GetLearningObjectSummary(int summaryId)
        {
            return _dbContext.LearningObjectSummaries.FirstOrDefault(los => los.Id == summaryId);
        }
    }
}