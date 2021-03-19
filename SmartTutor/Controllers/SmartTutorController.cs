using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.Communication;
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
        public ClassQualityAnalysisResponse GetCodeQualityAnalysis([FromBody] Guid Id)
        {
            ReportMessagesClass reportMessagesClass = ReportMessagesClass.Instance;
            var qualityAnalysisResponse = new ClassQualityAnalysisResponse();
            EducationContent contentForId = new EducationContent();
            try
            {  
                Dictionary<Guid, EducationContent> ReportMessages = reportMessagesClass.ReportMessages;
                contentForId = ReportMessages[Id];
                qualityAnalysisResponse.NewContent = contentForId;
                qualityAnalysisResponse.Id = Id;
            }
            catch
            {
               // TODO: Write some exp
            }
          
            return qualityAnalysisResponse;
        }

    }
}
