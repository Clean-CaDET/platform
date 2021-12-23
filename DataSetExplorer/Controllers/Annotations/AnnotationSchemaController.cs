using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using DataSetExplorer.Annotations;
using DataSetExplorer.Annotations.Model;
using DataSetExplorer.Controllers.Annotations.DTOs;

namespace DataSetExplorer.Controllers.Annotations
{
    [Route("api/annotation-schema/")]
    [ApiController]
    public class AnnotationSchemaController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAnnotationSchemaService _annotationSchemaService;

        public AnnotationSchemaController(IMapper mapper, IConfiguration configuration, IAnnotationSchemaService annotationSchemaService)
        {
            _mapper = mapper;
            _configuration = configuration;
            _annotationSchemaService = annotationSchemaService;
        }

        [HttpGet]
        [Route("code-smell-definition")]
        public IActionResult GetAllCodeSmellDefinitions()
        {
            var result = _annotationSchemaService.GetAllCodeSmellDefinitions();
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetCodeSmellDefinition([FromRoute] int id)
        {
            var result = _annotationSchemaService.GetCodeSmellDefinition(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpPost]
        [Route("code-smell-definition")]
        public IActionResult CreateCodeSmellDefinition([FromBody] CodeSmellDefinitionDTO codeSmellDefinitionDto)
        {
            var codeSmellDefinition = _mapper.Map<CodeSmellDefinition>(codeSmellDefinitionDto);
            var result = _annotationSchemaService.CreateCodeSmellDefinition(codeSmellDefinition);
            if (result.IsFailed) return NotFound(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpPost]
        [Route("code-smell-definition/{id}/heuristic")]
        public IActionResult AddHeuristicsToCodeSmell([FromRoute] int id, [FromBody] List<HeuristicDTO> heuristicsDto)
        {
            var heuristics = new List<Heuristic>();
            foreach (var heuristic in heuristicsDto)
            {
                heuristics.Add(_mapper.Map<Heuristic>(heuristic));
            }
            var result = _annotationSchemaService.AddHeuristicsToCodeSmell(id, heuristics);
            if (result.IsFailed) return NotFound(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpDelete]
        [Route("code-smell-definition/{smellId}/heuristic/{heuristicId}")]
        public IActionResult RemoveHeuristicFromCodeSmell([FromRoute] int smellId, [FromRoute] int heuristicId)
        {
            var result = _annotationSchemaService.DeleteHeuristicFromCodeSmell(smellId, heuristicId);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpGet]
        [Route("code-smell-definition/{id}/heuristic")]
        public IActionResult GetHeuristicsForCodeSmell([FromRoute] int id)
        {
            var result = _annotationSchemaService.GetHeuristicsForCodeSmell(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpPut]
        [Route("code-smell-definition/{id}")]
        public IActionResult UpdateCodeSmellDefinition([FromRoute] int id, [FromBody] CodeSmellDefinitionDTO codeSmellDefinitionDto)
        {
            var codeSmellDefinition = _mapper.Map<CodeSmellDefinition>(codeSmellDefinitionDto);
            var result = _annotationSchemaService.UpdateCodeSmellDefinition(id, codeSmellDefinition);
            if (result.IsFailed) return NotFound(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpDelete]
        [Route("code-smell-definition/{id}")]
        public IActionResult DeleteCodeSmellDefinition([FromRoute] int id)
        {
            var result = _annotationSchemaService.DeleteCodeSmellDefinition(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpGet]
        [Route("heuristic")]
        public IActionResult GetAllHeuristics()
        {
            var result = _annotationSchemaService.GetAllHeuristics();
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpPut]
        [Route("heuristic/{id}")]
        public IActionResult UpdateHeuristic([FromRoute] int id, [FromBody] HeuristicDTO heuristicDto)
        {
            var heuristic = _mapper.Map<Heuristic>(heuristicDto);
            var result = _annotationSchemaService.UpdateHeuristic(id, heuristic);
            if (result.IsFailed) return NotFound(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpDelete]
        [Route("heuristic/{id}")]
        public IActionResult DeleteHeuristic([FromRoute] int id)
        {
            var result = _annotationSchemaService.DeleteHeuristic(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpPost]
        [Route("heuristic")]
        public IActionResult CreateHeuristic([FromBody] HeuristicDTO heuristicDto)
        {
            var heuristic = _mapper.Map<Heuristic>(heuristicDto);
            var result = _annotationSchemaService.CreateHeuristic(heuristic);
            if (result.IsFailed) return NotFound(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }
    }
}
