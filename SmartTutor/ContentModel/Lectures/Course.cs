using System.Collections.Generic;

namespace SmartTutor.ContentModel.Lectures
{
    public class Course
    {
        public int Id { get; private set; }
        public List<Lecture> Lectures { get; private set; }
        public string Name { get; set; }

        public Course(int id, List<Lecture> lectures, string name)
        {
            Lectures = lectures;
            Name = name;
            Id = id;
        }
        
        public Course(string name)
        {
            Name = name;
            Lectures = new List<Lecture>();
        }

        private Course()
        {
        }

        public void AddLecture(Lecture lecture)
        {
            if (Lectures == null) Lectures = new List<Lecture>();
            Lectures.Add(lecture);
        }
    }
}