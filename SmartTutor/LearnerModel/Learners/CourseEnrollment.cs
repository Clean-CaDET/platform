using System;
using SmartTutor.ContentModel.Lectures;

namespace SmartTutor.LearnerModel.Learners
{
    public class CourseEnrollment
    {
        public int Id { get; private set; }
        public int CourseId { get; private set; }

        public CourseEnrollment(int id, int courseId)
        {
            CourseId = courseId;
            Id = id;
        }
    }
}