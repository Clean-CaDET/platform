using SmartTutor.Controllers.Content.DTOs;
using System.Collections.Generic;

namespace SmartTutor.Controllers.Progress.DTOs.SubmissionEvaluation
{
    public class ArrangeTaskContainerEvaluationDTO
    {
        public int Id { get; set; }
        public bool SubmissionWasCorrect { get; set; }
        public List<ArrangeTaskElementDTO> CorrectElements { get; set; }
    }
}