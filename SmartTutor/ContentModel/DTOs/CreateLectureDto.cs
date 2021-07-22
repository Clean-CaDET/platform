namespace SmartTutor.ContentModel.DTOs
{
    public class CreateLectureDto
    {
        public int TeacherId { get; set; }
        public int CourseId { get; set; } 
        public string LectureName { get; set; }
        public string LectureDescription { get; set; }

        public CreateLectureDto()
        {
        }
    }
}