using System.Collections.Generic;

namespace SmartTutor.Controllers.DTOs.Lecture
{
    public class KnowledgeNodeDTO
    {
        public int Id { get; set; }
        public string Goal { get; set; }
        public List<LearningObjectDTO> LearningObjects { get; set; }
        //TODO: Expand to include progress tracking information (locked, unlocked etc.) for logged in users.
        //KN type and LO are probably not important to the front-end so we will not include them in the DTO.
    }
}