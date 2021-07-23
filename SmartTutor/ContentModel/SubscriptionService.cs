using System;
using System.Collections.Generic;
using SmartTutor.ContentModel.DTOs;
using SmartTutor.ContentModel.Exceptions;
using SmartTutor.ContentModel.Lectures;
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

        public void SubscribeTeacher(SubscriptionDto dto)
        {
            var teacher = _subscriptionRepository.GetTeacher(dto.TeacherId);
            
            if (teacher.GetActiveSubscription() != null) throw new TeacherAlreadySubscribedException(dto.TeacherId.ToString());
            var individualPlan = _subscriptionRepository.GetIndividualPlan(dto.IndividualPlanId);
            var individualPlanUsage = new IndividualPlanUsage(individualPlan.Id);
            individualPlanUsage = _subscriptionRepository.SaveOrUpdatePlanUsage(individualPlanUsage);
            var end = GetEndDate(dto.NumberOfDays);
            var subscription = new Subscription(dto.TeacherId,DateTime.Now,end, individualPlanUsage.Id);
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
            var teacher = _subscriptionRepository.GetTeacher(teacherId);
            var subscription = teacher.GetActiveSubscription();
            var planUsage = _subscriptionRepository.GetIndividualPlanUsage(subscription.PlanUsageId);
            planUsage.IncrementNumberOfUsedLectures();
            _subscriptionRepository.SaveOrUpdatePlanUsage(planUsage);
        }

        public void IncrementNumberOfCourses(int teacherId)
        {
            var teacher = _subscriptionRepository.GetTeacher(teacherId);
            var subscription = teacher.GetActiveSubscription();
            var planUsage = _subscriptionRepository.GetIndividualPlanUsage(subscription.PlanUsageId);
            planUsage.IncrementNumberOfUsedCourses();
            _subscriptionRepository.SaveOrUpdatePlanUsage(planUsage);
        }

        public void AddCourseToTeacher(int teacherId, Course course)
        {
            var teacher = _subscriptionRepository.GetTeacher(teacherId);
            teacher.AddCourse(course);
            _subscriptionRepository.SaveOrUpdateTeacher(teacher);
        }

        private IndividualPlanUsage GetIndividualPlanUsage(int teacherId)
        {
            var teacher = _subscriptionRepository.GetTeacher(teacherId);
            var subscription = teacher.GetActiveSubscription();
            var planUsage = _subscriptionRepository.GetIndividualPlanUsage(subscription.PlanUsageId);
            return planUsage;
        }
        
        private static DateTime GetEndDate(int numberOfDays)
        {
            var end = DateTime.Now;
            Dictionary<int, int> mapOfDays = new Dictionary<int, int>();
            mapOfDays[30] = 30;
            mapOfDays[365] = 365;
            if (!mapOfDays.ContainsKey(numberOfDays))
            {
                throw new NumberOfDaysNotSupportedException(numberOfDays);
            }
            var days = mapOfDays[numberOfDays];
            end = end.AddDays(days);
            return end;
        }
    }
}