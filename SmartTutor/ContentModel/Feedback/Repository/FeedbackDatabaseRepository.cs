using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmartTutor.Database;

namespace SmartTutor.ContentModel.Feedback.Repository
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

        public void UpdateFeedback(LearningObjectFeedback feedback)
        {
            _dbContext.LearningObjectFeedbacks.Update(feedback);
            _dbContext.SaveChanges();
        }

        public LearningObjectFeedback GetFeedback(int learningObjectId, int traineeId)
        {
            return _dbContext.LearningObjectFeedbacks.AsNoTracking().FirstOrDefault(feedback =>
                feedback.LearningObjectId == learningObjectId && feedback.TraineeId == traineeId);
        }
    }
}