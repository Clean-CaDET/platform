using System;

namespace SmartTutor.ContentModel.Exceptions
{
    public class TeacherAlreadySubscribedException: Exception
    {
        public TeacherAlreadySubscribedException(string id): base(
            $"Teacher with id: {id} is already subscribed to course.")
        {
            
        }
    }
}