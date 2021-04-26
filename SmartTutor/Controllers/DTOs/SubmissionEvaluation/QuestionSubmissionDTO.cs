using SmartTutor.Controllers.DTOs.Content;
using System.Collections.Generic;

namespace SmartTutor.Controllers.DTOs.SubmissionEvaluation
{
    public class QuestionSubmissionDTO
    {
        public List<QuestionAnswerDTO> Answers { get; set; }
        public int LearnerId { get; set; }
        public int QuestionId { get; set; }
    }
}