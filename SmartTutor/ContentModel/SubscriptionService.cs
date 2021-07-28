using System;
using SmartTutor.ContentModel.Exceptions;
using SmartTutor.ContentModel.Lectures;
using SmartTutor.ContentModel.Subscriptions;
using SmartTutor.ContentModel.Subscriptions.Repository;

namespace SmartTutor.ContentModel
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public void SubscribeTeacher(Subscription subscription, int individualPlanId)
        {
            var teacher = _subscriptionRepository.GetTeacher(subscription.TeacherId);

            if (teacher.GetActiveSubscription() != null)
                throw new TeacherAlreadySubscribedException(subscription.TeacherId.ToString());
            var individualPlanUsage = new IndividualPlanUsage(individualPlanId);
            var individualPlan = _subscriptionRepository.GetIndividualPlan(individualPlanId);
            individualPlanUsage = _subscriptionRepository.SaveOrUpdatePlanUsage(individualPlanUsage);
            var end = GetEndDate(individualPlan);
            subscription.Start = DateTime.Now;
            subscription.End = end;
            subscription.IndividualPlanUsageId = individualPlanUsage.Id;
            subscription = _subscriptionRepository.SaveOrUpdateSubscription(subscription);
            teacher.AddSubscription(subscription);
            _subscriptionRepository.SaveOrUpdateTeacher(teacher);
        }

        public bool CanAddLecture(int teacherId)
        {
            var planUsage = GetIndividualPlanUsage(teacherId);
            var individualPlan = _subscriptionRepository.GetIndividualPlan(planUsage.IndividualPlanId);
            return individualPlan.NumberOfLectures - planUsage.NumberOfLecturesUsed >= 1;
        }

        public bool CanAddCourse(int teacherId)
        {
            var planUsage = GetIndividualPlanUsage(teacherId);
            var individualPlan = _subscriptionRepository.GetIndividualPlan(planUsage.IndividualPlanId);
            return individualPlan.NumberOfCourses - planUsage.NumberOfCoursesUsed >= 1;
        }

        public void IncrementNumberOfLectures(int teacherId)
        {
            var planUsage = GetIndividualPlanUsage(teacherId);
            planUsage.IncrementNumberOfUsedLectures();
            _subscriptionRepository.SaveOrUpdatePlanUsage(planUsage);
        }

        public void IncrementNumberOfCourses(int teacherId)
        {
            _subscriptionRepository.SaveOrUpdatePlanUsage(GetIndividualPlanUsage(teacherId));
        }

        public void AddCourseToTeacher(int teacherId, Course course)
        {
            var teacher = _subscriptionRepository.GetTeacher(teacherId);
            teacher.AddCourse(course);
            _subscriptionRepository.SaveOrUpdateTeacher(teacher);
            IncrementNumberOfCourses(teacherId);
        }

        private IndividualPlanUsage GetIndividualPlanUsage(int teacherId)
        {
            var teacher = _subscriptionRepository.GetTeacher(teacherId);
            var subscription = teacher.GetActiveSubscription();
            var planUsage = _subscriptionRepository.GetIndividualPlanUsage(subscription.IndividualPlanUsageId);
            return planUsage;
        }

        private static DateTime GetEndDate(IndividualPlan plan)
        {
            var end = DateTime.Now;
            end = end.Add(plan.Duration);
            return end;
        }
    }
}