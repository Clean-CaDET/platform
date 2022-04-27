using System.Collections.Generic;
using AutoMapper;
using DataSetExplorer.Core.AnnotationSchema;
using DataSetExplorer.Core.AnnotationSchema.Model;
using DataSetExplorer.UI.Controllers.AnnotationSchema.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DataSetExplorer.UI.Controllers.AnnotationSchema
{
    [Route("api/annotation-schema/code-smells/")]
    [ApiController]
    public class CodeSmellDefinitionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAnnotationSchemaService _annotationSchemaService;

        public CodeSmellDefinitionController(IMapper mapper, IAnnotationSchemaService annotationSchemaService)
        {
            _mapper = mapper;
            _annotationSchemaService = annotationSchemaService;
        }

        [HttpGet]
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
        public IActionResult CreateCodeSmellDefinition([FromBody] CodeSmellDefinitionDTO codeSmellDefinitionDto)
        {
            var codeSmellDefinition = _mapper.Map<CodeSmellDefinition>(codeSmellDefinitionDto);
            var result = _annotationSchemaService.CreateCodeSmellDefinition(codeSmellDefinition);
            if (result.IsFailed) return NotFound(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateCodeSmellDefinition([FromRoute] int id, [FromBody] CodeSmellDefinitionDTO codeSmellDefinitionDto)
        {
            var codeSmellDefinition = _mapper.Map<CodeSmellDefinition>(codeSmellDefinitionDto);
            var result = _annotationSchemaService.UpdateCodeSmellDefinition(id, codeSmellDefinition);
            if (result.IsFailed) return NotFound(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteCodeSmellDefinition([FromRoute] int id)
        {
            var result = _annotationSchemaService.DeleteCodeSmellDefinition(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpGet]
        [Route("{id}/heuristics")]
        public IActionResult GetHeuristicsForCodeSmell([FromRoute] int id)
        {
            var result = _annotationSchemaService.GetHeuristicsForCodeSmell(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpPost]
        [Route("{id}/heuristics")]
        public IActionResult AddHeuristicsToCodeSmell([FromRoute] int id, [FromBody] List<HeuristicDefinitionDTO> heuristicsDto)
        {
            var heuristics = new List<HeuristicDefinition>();
            foreach (var heuristic in heuristicsDto)
            {
                heuristics.Add(_mapper.Map<HeuristicDefinition>(heuristic));
            }
            var result = _annotationSchemaService.AddHeuristicsToCodeSmell(id, heuristics);
            if (result.IsFailed) return NotFound(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpDelete]
        [Route("{smellId}/heuristics/{heuristicId}")]
        public IActionResult RemoveHeuristicFromCodeSmell([FromRoute] int smellId, [FromRoute] int heuristicId)
        {
            var result = _annotationSchemaService.DeleteHeuristicFromCodeSmell(smellId, heuristicId);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }
    }
}
