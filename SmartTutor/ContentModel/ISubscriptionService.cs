using SmartTutor.ContentModel.Lectures;
using SmartTutor.ContentModel.Subscriptions;

namespace SmartTutor.ContentModel
{
    public interface ISubscriptionService
    {
        void SubscribeTeacher(Subscription subscription, int individualPLanId);
        bool CanAddLecture(int teacherId);
        bool CanAddCourse(int teacherId);
        void IncrementNumberOfLectures(int teacherId);
        void IncrementNumberOfCourses(int teacherId);
        void AddCourseToTeacher(int teacherId, Course course);
    }
}