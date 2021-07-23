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

        public Subscription SaveOrUpdateSubscription(Subscription subscription)
        {
            var s = _dbContext.Subscriptions.Attach(subscription).Entity;
            _dbContext.SaveChanges();
            return s;
        }

        public Teacher SaveOrUpdateTeacher(Teacher teacher)
        {
            var t = _dbContext.Teachers.Attach(teacher).Entity;
            _dbContext.SaveChanges();
            return t;
        }

        public IndividualPlanUsage SaveOrUpdatePlanUsage(IndividualPlanUsage individualPlanUsage)
        {
            var i = _dbContext.IndividualPlanUsages.Attach(individualPlanUsage).Entity;
            _dbContext.SaveChanges();
            return i;
        }

        public Teacher GetTeacher(int id)
        {
            return _dbContext.Teachers.Where(teacher => teacher.Id.Equals(id)).Include(teacher => teacher.Subscriptions).FirstOrDefault();
        }

        public IndividualPlan GetIndividualPlan(int id)
        {
            return _dbContext.IndividualPlans.FirstOrDefault(plan => plan.Id.Equals(id));
        }

        public IndividualPlanUsage GetIndividualPlanUsage(int id)
        {
            return _dbContext.IndividualPlanUsages.FirstOrDefault(plan => plan.Id.Equals(id));
        }
    }
}