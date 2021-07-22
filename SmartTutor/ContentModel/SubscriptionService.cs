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
        private static Dictionary<int, int> _mapOfDays; 

        public SubscriptionService(ISubscriptionRepository subscriptionRepository, Dictionary<int, int> mapOfDays)
        {
            _subscriptionRepository = subscriptionRepository;
            _mapOfDays = mapOfDays;
            _mapOfDays[30] = 30;
            _mapOfDays[365] = 365;
        }

        public void SubscribeTeacher(SubscriptionDto dto)
        {
            var teacher = _subscriptionRepository.GetTeacher(dto.TeacherId);
            
            if (teacher.GetActiveSubscription() != null) throw new TeacherAlreadySubscribedException(dto.TeacherId.ToString());
            var individualPlan = _subscriptionRepository.GetIndividualPlan(dto.IndividualPlanId);
            var individualPlanUsage = new IndividualPlanUsage(individualPlan);
            var end = GetEndDate(dto.NumberOfDays);
            var subscription = new Subscription(dto.TeacherId,new DateTime(),end, individualPlanUsage);
            
            teacher.AddSubscription(subscription);
            _subscriptionRepository.SaveOrUpdateTeacher(teacher);
            _subscriptionRepository.SaveOrUpdatePlanUsage(individualPlanUsage);
            _subscriptionRepository.SaveOrUpdateSubscription(subscription);
        }

        public bool CanAddLecture(int teacherId)
        {
            var planUsage = GetIndividualPlanUsage(teacherId);
            return planUsage.NumberOfLecturesLeft() >= 1;
        }

        public bool CanAddCourse(int teacherId)
        {
            var planUsage = GetIndividualPlanUsage(teacherId);
            return planUsage.NumberOfCoursesLeft() >= 1;
        }

        public void IncrementNumberOfLectures(int teacherId)
        {
            var teacher = _subscriptionRepository.GetTeacher(teacherId);
            var subscription = teacher.GetActiveSubscription();
            var planUsage = subscription.PlanUsage;
            planUsage.IncrementNumberOfUsedLectures();
            subscription.PlanUsage = planUsage;
            _subscriptionRepository.SaveOrUpdatePlanUsage(planUsage);
        }

        public void IncrementNumberOfCourses(int teacherId)
        {
            var teacher = _subscriptionRepository.GetTeacher(teacherId);
            var subscription = teacher.GetActiveSubscription();
            var planUsage = subscription.PlanUsage;
            planUsage.IncrementNumberOfUsedCourses();
            subscription.PlanUsage = planUsage;
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
            var planUsage = subscription.PlanUsage;
            return planUsage;
        }


        private static DateTime GetEndDate(int numberOfDays)
        {
            var end = new DateTime();
            if (!_mapOfDays.ContainsKey(numberOfDays))
            {
                throw new NumberOfDaysNotSupportedException(numberOfDays);
            }
            var days = _mapOfDays[numberOfDays];
            end = end.AddDays(days);
            return end;
        }
    }
}