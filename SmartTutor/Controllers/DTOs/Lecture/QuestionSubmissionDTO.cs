using System.Collections.Generic;

namespace SmartTutor.Controllers.DTOs.Lecture
{
    //TODO: Reorganize DTOs to have submissions and evaluations in one segment, while static content is in another
    public class QuestionSubmissionDTO
    {
        public List<QuestionAnswerDTO> Answers { get; set; }
        public int TraineeId { get; set; }
        public int QuestionId { get; set; }
    }
}