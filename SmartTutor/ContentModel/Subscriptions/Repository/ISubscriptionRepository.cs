namespace SmartTutor.ContentModel.Subscriptions.Repository
{
    public interface ISubscriptionRepository
    {
        void saveOrUpdateSubscription(Subscription subscription);
        void saveOrUpdateTeacher(Teacher teacher);
    }
}