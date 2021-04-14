using SmartTutor.Database;

namespace SmartTutor.ContentModel.FeedbackModel.Repository
{
    public class FeedbackDatabaseRepository : IFeedbackRepository
    {
        private readonly SmartTutorContext _dbContext;

        public FeedbackDatabaseRepository(SmartTutorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SaveFeedback(LearningObjectFeedback feedback)
        {
            _dbContext.LearningObjectFeedbacks.Add(feedback);
            _dbContext.SaveChanges();
        }
    }
}