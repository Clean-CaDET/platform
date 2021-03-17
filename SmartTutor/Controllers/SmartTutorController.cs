using System;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.ContentModel;
using SmartTutor.Controllers.DTOs;
using SmartTutor.Repository;
using SmartTutor.Service;

namespace SmartTutor.Controllers
{
    [Route("api/smarttutor")]
    [ApiController]
    public class SmarttutorController : ControllerBase
    {
        public ContentService ContentService;

        public SmarttutorController()
        {
            // Change param in constructor for ContentService if you want to get some other repository implementation
            ContentService = new ContentService(new ContentInMemoryRepository());
        }

        [HttpPost("education/class")]
        public ClassQualityAnalysisResponse GetCodeQualityAnalysis([FromBody] string classCode)
        {
            var content = MakeGodClassContent();

            return new ClassQualityAnalysisResponse()
            {
                NewContent = content
            };
        }

        // TODO: DELETE THIS, THIS IS ONLY FOR TEST PURPOSE
        private EducationContent MakeGodClassContent()
        {
            var content = ContentService.FindContentForIssue(SmellType.GOD_CLASS, 0);
            return content;
        }
    }
}
