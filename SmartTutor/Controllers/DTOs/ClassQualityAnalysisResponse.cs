using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartTutor.ContentModel;

namespace SmartTutor.Controllers.DTOs
{
    public class ClassQualityAnalysisResponse
    {
        public Guid Id { get; set; }

        public EducationContent NewContent { get; set; }
    }
}
