using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.Controllers.KnowledgeComponent.DTOs;
using SmartTutor.KnowledgeComponentModel;

namespace SmartTutor.Controllers.KnowledgeComponent
{
    [Route("api/knowledge-components/")]
    [ApiController]
    public class KnowledgeComponentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IKnowledgeComponentService _knowledgeComponentService;

        public KnowledgeComponentController(IMapper mapper, IKnowledgeComponentService knowledgeComponentService)
        {
            _mapper = mapper;
            _knowledgeComponentService = knowledgeComponentService;
        }

        [HttpGet]
        public ActionResult<List<KnowledgeComponentDTO>> GetKnowledgeComponents()
        {
            var knowledgeComponents = _knowledgeComponentService.GetKnowledgeComponents();
            return Ok(knowledgeComponents.Select(kc => _mapper.Map<KnowledgeComponentDTO>(kc)).ToList());
        }
    }
}