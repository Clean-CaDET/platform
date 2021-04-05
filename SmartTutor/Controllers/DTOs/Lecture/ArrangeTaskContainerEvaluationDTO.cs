using System.Collections.Generic;

namespace SmartTutor.Controllers.DTOs.Lecture
{
    public class ArrangeTaskContainerEvaluationDTO
    {
        public int Id { get; set; }
        public bool SubmissionWasCorrect { get; set; }
        public List<ArrangeTaskElementDTO> CorrectElements { get; set; }
    }
}