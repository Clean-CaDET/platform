namespace SmartTutor.ContentModel.DTOs
{
    public class CreateCourseDto
    {
        public int TeacherId { get; set; }
        public string CourseName { get; set; }

        public CreateCourseDto()
        {
        }
    }
}