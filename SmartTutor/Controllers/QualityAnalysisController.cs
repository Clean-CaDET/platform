using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.Controllers.DTOs.QualityAnalysis;
using SmartTutor.QualityAnalysis;

namespace SmartTutor.Controllers
{
    [Route("api/code-analysis/")]
    [ApiController]
    public class QualityAnalysisController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICodeQualityAnalyzer _qualityAnalyzerService;

        public QualityAnalysisController(IMapper mapper, ICodeQualityAnalyzer service)
        {
            _mapper = mapper;
            _qualityAnalyzerService = service;
        }

        [HttpPost]
        public ActionResult<CodeEvaluationDTO> Evaluate([FromBody] CodeSubmissionDTO codeSubmission)
        {
            var evaluation = _qualityAnalyzerService.EvaluateCode(_mapper.Map<CodeSubmission>(codeSubmission));
            return Ok(_mapper.Map<CodeEvaluationDTO>(evaluation));
        }
    }
}
