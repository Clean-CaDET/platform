namespace SmartTutor.ProgressModel.Progress
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