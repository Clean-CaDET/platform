using System.Collections.Generic;

namespace SmartTutor.ContentModel.Subscriptions.Repository
{
    public interface ISubscriptionRepository
    {
        Subscription SaveOrUpdateSubscription(Subscription subscription);
        Teacher SaveOrUpdateTeacher(Teacher teacher);
        IndividualPlanUsage SaveOrUpdatePlanUsage(IndividualPlanUsage individualPlanUsage);
        Teacher GetTeacher(int id);
        IndividualPlan GetIndividualPlan(int id);
        IndividualPlanUsage GetIndividualPlanUsage(int id);
        List<IndividualPlan> GetAllIndividualPlans();
    }
}