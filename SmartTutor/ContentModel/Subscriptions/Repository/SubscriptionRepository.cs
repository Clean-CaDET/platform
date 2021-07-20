using SmartTutor.Database;

namespace SmartTutor.ContentModel.Subscriptions.Repository
{
    public class SubscriptionRepository:ISubscriptionRepository
    {
        private readonly SmartTutorContext _dbContext;

        public SubscriptionRepository(SmartTutorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void saveOrUpdateSubscription(Subscription subscription)
        {
            _dbContext.Subscriptions.Attach(subscription);
            _dbContext.SaveChanges();
        }

        public void saveOrUpdateTeacher(Teacher teacher)
        {
            _dbContext.Teachers.Attach(teacher);
            _dbContext.SaveChanges();
        }
    }
}