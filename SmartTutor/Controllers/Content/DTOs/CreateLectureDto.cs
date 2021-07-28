namespace SmartTutor.Controllers.Content.DTOs
{
    public class CreateLectureDto
    {
        public int TeacherId { get; set; }
        public LectureDTO Lecture { get; set; }

        public CreateLectureDto()
        {
        }
    }
}