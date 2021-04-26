using SmartTutor.Controllers.DTOs.Content;
using System.Collections.Generic;

namespace SmartTutor.Controllers.DTOs.SubmissionEvaluation
{
    public class ArrangeTaskContainerEvaluationDTO
    {
        public int Id { get; set; }
        public bool SubmissionWasCorrect { get; set; }
        public List<ArrangeTaskElementDTO> CorrectElements { get; set; }
    }
}