using SmartTutor.ContentModel.Exceptions;
using SmartTutor.ContentModel.Subscriptions;
using SmartTutor.ContentModel.Subscriptions.Repository;

namespace SmartTutor.ContentModel
{
    public class SubscriptionService:ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public void SubscribeTeacher(Teacher teacher, Subscription subscription)
        {
            if (teacher.GetActiveSubscription() == null) throw new TeacherAlreadySubscribedException(teacher.Id.ToString());
            teacher.AddSubscription(subscription);
            _subscriptionRepository.saveOrUpdateSubscription(subscription);
            _subscriptionRepository.saveOrUpdateTeacher(teacher);
        }
    }
}