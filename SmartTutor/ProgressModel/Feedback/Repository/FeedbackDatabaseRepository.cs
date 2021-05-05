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
            _dbContext.LearningObjectFeedback.Attach(feedback);
            _dbContext.SaveChanges();
        }

        public LearningObjectFeedback Get(int learningObjectId, int learnerId)
        {
            return _dbContext.LearningObjectFeedback.FirstOrDefault(feedback =>
                feedback.LearningObjectId == learningObjectId && feedback.LearnerId == learnerId);
        }
    }
}