using System.Collections.Generic;

namespace SmartTutor.Controllers.Content.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }
        public List<LectureDTO> Lectures { get; set; }
        public string Name { get; set; }

        public CourseDto()
        {
        }
    }
}