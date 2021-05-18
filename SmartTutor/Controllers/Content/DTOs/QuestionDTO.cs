using System.Collections.Generic;

namespace SmartTutor.Controllers.Content.DTOs
{
    public class QuestionDTO : LearningObjectDTO
    {
        public string Text { get; set; }
        public List<QuestionAnswerDTO> PossibleAnswers { get; set; }
    }
}