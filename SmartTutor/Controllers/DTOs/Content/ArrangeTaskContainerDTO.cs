using System.Collections.Generic;

namespace SmartTutor.Controllers.DTOs.Content
{
    public class ArrangeTaskContainerDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<ArrangeTaskElementDTO> Elements { get; set; }
    }
}