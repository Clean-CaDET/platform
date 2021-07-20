using SmartTutor.ContentModel.Subscriptions;

namespace SmartTutor.ContentModel
{
    public interface ISubscriptionService
    {
        void SubscribeTeacher(Teacher teacher, Subscription subscription);
        //void MakeCourse(Teacher teacher, Course course);
        /*//TODO too many params (logic in other service?)
        void AddLectureToCourse(Teacher teacher, Course course, Lecture lecture);*/
    }
}