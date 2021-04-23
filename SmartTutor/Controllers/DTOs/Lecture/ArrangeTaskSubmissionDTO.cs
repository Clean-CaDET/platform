using System.Collections.Generic;

namespace SmartTutor.Controllers.DTOs.Lecture
{
    public class ArrangeTaskSubmissionDTO
    {
        public int ArrangeTaskId { get; set; }
        public int LearnerId { get; set; }
        public List<ArrangeTaskContainerDTO> Containers { get; set; }
    }
}