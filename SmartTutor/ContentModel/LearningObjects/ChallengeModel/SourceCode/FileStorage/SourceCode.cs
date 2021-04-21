using System;
using System.Collections.Generic;

namespace Methods._03._Parameter_Lists
{
    /// <summary>
    /// 1. Apply the strategies for parameter list reduction for the methods in the following classes.
    /// </summary>
    class CourseService
    {
        private readonly List<Student> _students = new List<Student>();
        public void RegisterStudent(string name, string surname, DateTime dateOfBirth, string index)
        {
            var newStudent = new Student(name, surname, dateOfBirth, index);
            foreach (var student in _students)
            {
                if(student.Index.Equals(index)) throw new InvalidOperationException("Student exists.");
            }
            _students.Add(newStudent);
        }
        public void Enroll(Student student, Course newCourse, int newCourseESPB)
        {
            int numberOfActiveCourses = 0;
            foreach (var c in student.Courses)
            {
                if (IsActive(c)) numberOfActiveCourses++;
            }

            if (numberOfActiveCourses < 7 && student.ActiveESPB < 80)
            {
                student.Courses.Add(newCourse);
                student.ActiveESPB += newCourseESPB;
            }
        }

        public bool IsActive(Course c)
        {
            return c.Status.Equals("enrolled") || c.Status.Equals("current");
        }
    }

    internal class Course
    {
        public string Status { get; set; }
    }

    internal class Student
    {
        public Student(string name, string surname, DateTime dateOfBirth, string index)
        {
            Name = name;
            Surname = surname;
            DateOfBirth = dateOfBirth;
            Index = index;
            ActiveESPB = 0;
        }
        
        public List<Course> Courses { get; set; }
        public string Name { get; }
        public string Surname { get; }
        public DateTime DateOfBirth { get; }
        public string Index { get; }
        public int ActiveESPB { get; set; }
    }
}
