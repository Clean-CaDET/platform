using Microsoft.EntityFrameworkCore;
using SmartTutor.Database;
using System.Linq;

namespace SmartTutor.ProgressModel.Feedback.Repository
{
    public class FeedbackDatabaseRepository : IFeedbackRepository
    {
        private readonly SmartTutorContext _dbContext;

        public FeedbackDatabaseRepository(SmartTutorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SaveOrUpdate(LearningObjectFeedback feedback)
        {
            var loadedFeedback = Get(feedback.LearningObjectId, feedback.LearnerId);
            if (loadedFeedback == null) _dbContext.LearningObjectFeedbacks.Add(feedback);
            else _dbContext.LearningObjectFeedbacks.Update(feedback);

            _dbContext.SaveChanges();
        }

        public LearningObjectFeedback Get(int learningObjectId, int learnerId)
        {
            return _dbContext.LearningObjectFeedbacks.AsNoTracking().FirstOrDefault(feedback =>
                feedback.LearningObjectId == learningObjectId && feedback.LearnerId == learnerId);
        }
    }
}