using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        public void SaveOrUpdateSubscription(Subscription subscription)
        {
            _dbContext.Subscriptions.Attach(subscription);
            _dbContext.SaveChanges();
        }

        public void SaveOrUpdateTeacher(Teacher teacher)
        {
            _dbContext.Teachers.Attach(teacher);
            _dbContext.SaveChanges();
        }

        public void SaveOrUpdatePlanUsage(IndividualPlanUsage individualPlanUsage)
        {
            _dbContext.IndividualPlanUsages.Attach(individualPlanUsage);
            _dbContext.SaveChanges();
        }

        public Teacher GetTeacher(int id)
        {
            return _dbContext.Teachers.Where(teacher => teacher.Id.Equals(id)).Include(teacher => teacher.Subscriptions).FirstOrDefault();
        }

        public IndividualPlan GetIndividualPlan(int id)
        {
            return _dbContext.IndividualPlans.FirstOrDefault(plan => plan.Id.Equals(id));
        }
    }
}