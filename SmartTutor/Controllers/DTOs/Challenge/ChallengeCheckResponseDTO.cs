namespace SmartTutor.Controllers.DTOs.Challenge
{
    public class ChallengeCheckResponseDTO
    {
        public string ResponseText { get; set; }

        public ChallengeCheckResponseDTO(string text)
        {
            ResponseText = text;
        }
    }
}