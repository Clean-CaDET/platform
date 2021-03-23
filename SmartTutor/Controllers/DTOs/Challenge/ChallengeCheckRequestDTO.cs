namespace SmartTutor.Controllers.DTOs.Challenge
{
    public class ChallengeCheckRequestDTO
    {
        public string[] SourceCode { get; set; }
        public int ChallengeId { get; set; }
        public string StudentId { get; set; }
    }
}
