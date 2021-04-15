using System.Collections.Generic;

namespace SmartTutor.Controllers.DTOs.Lecture
{
    public class ArrangeTaskSubmissionDTO
    {
        public int ArrangeTaskId { get; set; }
        public int TraineeId { get; set; }
        //TODO: Should introduce another DTO to promote better decoupling between static content and submission/evaluations.
        public List<ArrangeTaskContainerDTO> Containers { get; set; }
    }
}