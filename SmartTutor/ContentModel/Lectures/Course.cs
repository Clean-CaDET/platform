using System.Collections.Generic;

namespace SmartTutor.ContentModel.Lectures
{
    public class Course
    {
        public int Id { get; private set; }
        public List<Lecture> Lectures { get; private set; }

        public Course(int id, List<Lecture> lectures)
        {
            Lectures = lectures;
            Id = id;
        }

        private Course()
        {
        }
    }
}