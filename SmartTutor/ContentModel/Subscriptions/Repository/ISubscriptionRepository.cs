namespace SmartTutor.ContentModel.Subscriptions.Repository
{
    public interface ISubscriptionRepository
    {
        void SaveOrUpdateSubscription(Subscription subscription);
        void SaveOrUpdateTeacher(Teacher teacher);
        void SaveOrUpdatePlanUsage(IndividualPlanUsage individualPlanUsage);


        Teacher GetTeacher(int id);
        IndividualPlan GetIndividualPlan(int id);
    }
}