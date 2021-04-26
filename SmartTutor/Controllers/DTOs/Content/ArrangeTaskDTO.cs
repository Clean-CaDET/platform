using System.Collections.Generic;

namespace SmartTutor.Controllers.DTOs.Content
{
    public class ArrangeTaskDTO : LearningObjectDTO
    {
        public string Text { get; set; }
        public List<ArrangeTaskContainerDTO> Containers { get; set; }
        public List<ArrangeTaskElementDTO> UnarrangedElements { get; set; }
    }
}