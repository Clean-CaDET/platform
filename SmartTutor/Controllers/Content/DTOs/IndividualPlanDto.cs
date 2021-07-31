using System;

namespace SmartTutor.Controllers.Content.DTOs
{
    public class IndividualPlanDto
    {
        public int Id { get; set; }
        public int NumberOfUsers { get; set; }
        public int NumberOfCourses { get; set; }
        public int NumberOfLectures { get; set; }
        public TimeSpan Duration { get; set; }

        public IndividualPlanDto()
        {
        }

        public IndividualPlanDto(int id, int numberOfUsers, int numberOfCourses, int numberOfLectures,
            TimeSpan duration)
        {
            Id = id;
            NumberOfUsers = numberOfUsers;
            NumberOfCourses = numberOfCourses;
            NumberOfLectures = numberOfLectures;
            Duration = duration;
        }
    }
}