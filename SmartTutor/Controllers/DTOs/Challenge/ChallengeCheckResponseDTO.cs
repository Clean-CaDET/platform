namespace SmartTutor.Controllers.DTOs.Challenge
{
    public class ChallengeCheckResponseDTO
    {
        public string ResponseText { get; private set; }

        public ChallengeCheckResponseDTO(string text)
        {
            ResponseText = text;
        }
    }
}