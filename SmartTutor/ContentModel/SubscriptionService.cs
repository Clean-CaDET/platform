using System;
using System.Collections.Generic;
using SmartTutor.ContentModel.Exceptions;
using SmartTutor.ContentModel.Lectures;
using SmartTutor.ContentModel.Subscriptions;
using SmartTutor.ContentModel.Subscriptions.Repository;
using SmartTutor.Controllers.Content.DTOs;

namespace SmartTutor.ContentModel
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public void SubscribeTeacher(int teacherId, int individualPlanId)
        {
            var teacher = _subscriptionRepository.GetTeacher(teacherId);

            if (teacher.GetActiveSubscription() != null)
                throw new TeacherAlreadySubscribedException(teacherId.ToString());

            var individualPlanUsage = new IndividualPlanUsage(individualPlanId);
            var individualPlan = _subscriptionRepository.GetIndividualPlan(individualPlanId);
            individualPlanUsage = _subscriptionRepository.SaveOrUpdatePlanUsage(individualPlanUsage);
            var end = GetEndDate(individualPlan);
            var subscription = new Subscription(teacherId, DateTime.Now, end, individualPlanUsage.Id);
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
            var planUsage = GetIndividualPlanUsage(teacherId);
            planUsage.IncrementNumberOfUsedCourses();
            _subscriptionRepository.SaveOrUpdatePlanUsage(planUsage);
        }

        public void AddCourseToTeacher(int teacherId, Course course)
        {
            var teacher = _subscriptionRepository.GetTeacher(teacherId);
            teacher.AddCourse(course);
            _subscriptionRepository.SaveOrUpdateTeacher(teacher);
            IncrementNumberOfCourses(teacherId);
        }

        public List<IndividualPlanDto> GetIndividualPlans()
        {
            var plans = _subscriptionRepository.GetAllIndividualPlans();
            var dtos = new List<IndividualPlanDto>();
            foreach (var plan in plans)
            {
                dtos.Add(new IndividualPlanDto(plan.Id, plan.NumberOfUsers, plan.NumberOfCourses, plan.NumberOfLectures,
                    plan.Duration));
            }

            return dtos;
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