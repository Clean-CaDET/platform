namespace SmartTutor.Controllers.DTOs
{
    public class ChallengeCheckResponse
    {
        public string ResponseText { get; set; }

        public ChallengeCheckResponse(string text)
        {
            ResponseText = text;
        }
    }
}