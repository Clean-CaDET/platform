using System.Collections.Generic;

namespace SmartTutor.Controllers.DTOs.Lecture
{
    public class ArrangeTaskContainerDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<ArrangeTaskElementDTO> SubmittedElements { get; set; }
    }
}