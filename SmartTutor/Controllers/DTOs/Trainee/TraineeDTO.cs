namespace SmartTutor.Controllers.DTOs.Trainee
{
    public class TraineeDTO
    {
        public int Id { get; set; }
        public string StudentIndex { get; set; }
        public int VisualScore { get; set; }
        public int AuralScore { get; set; }
        public int ReadWriteScore { get; set; }
        public int KinaestheticScore { get; set; }
    }
}