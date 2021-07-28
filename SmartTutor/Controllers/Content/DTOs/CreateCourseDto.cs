namespace SmartTutor.Controllers.Content.DTOs
{
    public class CreateCourseDto
    {
        public CourseDto Course { get; set; }
        public int TeacherId { get; set; }

        public CreateCourseDto()
        {
        }
    }
}