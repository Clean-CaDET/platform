namespace SmartTutor.Controllers.DTOs
{
    public class ChallengeCheckRequest
    {
        public string[] SourceCode { get; set; }
        public int ChallengeId { get; set; }
        public string StudentId { get; set; }
    }
}
