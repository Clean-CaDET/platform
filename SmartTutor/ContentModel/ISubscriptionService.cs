using System.Collections.Generic;
using SmartTutor.ContentModel.Lectures;
using SmartTutor.ContentModel.Subscriptions;
using SmartTutor.Controllers.Content.DTOs;

namespace SmartTutor.ContentModel
{
    public interface ISubscriptionService
    {
        void SubscribeTeacher(int teacherId, int individualPLanId);
        bool CanAddLecture(int teacherId);
        bool CanAddCourse(int teacherId);
        void IncrementNumberOfLectures(int teacherId);
        void IncrementNumberOfCourses(int teacherId);
        void AddCourseToTeacher(int teacherId, Course course);
        List<IndividualPlanDto> GetIndividualPlans();
    }
}