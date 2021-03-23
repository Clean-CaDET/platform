using Microsoft.AspNetCore.Mvc;
using SmartTutor.Controllers.DTOs.Lecture;
using System.Collections.Generic;

namespace SmartTutor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LectureController : ControllerBase
    {
        [HttpGet("all")]
        public ActionResult<List<LectureDTO>> GetAllLectures()
        {
            return GetLectureList();
        }

        [HttpGet("get/{id}")]
        public ActionResult<LectureDTO> CheckChallengeCompletion(int id)
        {
            var lecture = GetFullLecture(id);
            if (lecture == null) NotFound();
            return lecture;
        }

        //TODO: The code below is temporary to enable integration with the Web UI functionality. It will be replaced with DB interaction.
        private List<LectureDTO> GetLectureList()
        {
            var lectures = new List<LectureDTO>
            {
                new LectureDTO {Id = 1, Name = "Good Naming"},
                new LectureDTO {Id = 2, Name = "Clean Methods"},
                new LectureDTO {Id = 3, Name = "Class - Cohesion"},
                new LectureDTO {Id = 4, Name = "Class - Coupling"},
                new LectureDTO {Id = 5, Name = "Single Responsibility Principle"}
            };
            return lectures;
        }

        private LectureDTO GetFullLecture(int id)
        {
            var lecture = new LectureDTO { Id = 3, Name = "Class - Cohesion" };
            if (id != lecture.Id) return null;

            var learningObjects = new List<LearningObjectDTO>();
            learningObjects.Add(new TextDTO { Id = 1, Text = "Cohesion determines the degree to which a part of a codebase forms a meaningful atomic module. The elements of a highly cohesive module work together towards a common, well-defined goal and have a clear (single) responsibility. This responsibility is defined by the module’s name and described by its interface that sets its inputs and outputs." });
            learningObjects.Add(new ImageDTO { Id = 2, Url = "https://miro.medium.com/max/2400/1*3jfye6OQFu_dROKb14BhaQ.png", Caption = "The left class is playing with a few responsibilities, more than its name suggests anyway…" });
            learningObjects.Add(new TextDTO { Id = 3, Text = "Structural cohesion is a metric that is calculated based on the number of connections between a module’s elements." });
            learningObjects.Add(new ImageDTO { Id = 4, Url = "https://miro.medium.com/max/700/1*OF4xmCDnuV_VRDcqiLp46Q.png", Caption = "How does this formula hold for data transfer object classes? What about classes without fields?" });
            learningObjects.Add(new TextDTO { Id = 5, Text = "Semantic cohesion determines the degree to which the elements of a module are semantically related." });
            learningObjects.Add(new VideoDTO { Id = 6, Url = "https://www.youtube.com/watch?v=qE-Gmu_YuQE" });

            var knowledgeNode = new List<KnowledgeNodeDTO>
            {
                new KnowledgeNodeDTO { Goal = "Understand the basics of structural and semantic cohesion.", Id = 1, LearningObjects = learningObjects }
            };

            lecture.KnowledgeNodes = knowledgeNode;

            return lecture;
        }
    }
}
