using AutoMapper;
using DataSetExplorer.Core.AnnotationSchema;
using DataSetExplorer.Core.AnnotationSchema.Model;
using DataSetExplorer.UI.Controllers.AnnotationSchema.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DataSetExplorer.UI.Controllers.AnnotationSchema
{
    [Route("api/annotation-schema/code-smells/")]
    [ApiController]
    public class AnnotationSchemaController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAnnotationSchemaService _annotationSchemaService;

        public AnnotationSchemaController(IMapper mapper, IAnnotationSchemaService annotationSchemaService)
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

        [HttpGet]
        [Route("name/{name}")]
        public IActionResult GetCodeSmellDefinitionByName([FromRoute] string name)
        {
            var result = _annotationSchemaService.GetCodeSmellDefinitionByName(name);
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

        [HttpGet]
        [Route("heuristics")]
        public IActionResult GetHeuristicsForEachCodeSmell()
        {
            var result = _annotationSchemaService.GetHeuristicsForEachCodeSmell();
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpPost]
        [Route("{id}/heuristics")]
        public IActionResult AddHeuristicToCodeSmell([FromRoute] int id, [FromBody] HeuristicDefinitionDTO heuristicDto)
        {
            var heuristic = _mapper.Map<HeuristicDefinition>(heuristicDto);
            var result = _annotationSchemaService.AddHeuristicToCodeSmell(id, heuristic);
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

        [HttpPut]
        [Route("{id}/heuristics")]
        public IActionResult UpdateHeuristicInCodeSmell([FromRoute] int id, [FromBody] HeuristicDefinitionDTO heuristicDto)
        {
            var heuristic = _mapper.Map<HeuristicDefinition>(heuristicDto);
            var result = _annotationSchemaService.UpdateHeuristicInCodeSmell(id, heuristic);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpGet]
        [Route("{id}/severities")]
        public IActionResult GetSeveritiesForCodeSmell([FromRoute] int id)
        {
            var result = _annotationSchemaService.GetSeveritiesForCodeSmell(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpGet]
        [Route("severities")]
        public IActionResult GetSeveritiesForEachCodeSmell()
        {
            var result = _annotationSchemaService.GetSeveritiesForEachCodeSmell();
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpPost]
        [Route("{id}/severities")]
        public IActionResult AddSeverityToCodeSmell([FromRoute] int id, [FromBody] SeverityDefinitionDTO severityDto)
        {
            var severity = _mapper.Map<SeverityDefinition>(severityDto);
            var result = _annotationSchemaService.AddSeverityToCodeSmell(id, severity);
            if (result.IsFailed) return NotFound(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpDelete]
        [Route("{smellId}/severities/{severityId}")]
        public IActionResult RemoveSeverityFromCodeSmell([FromRoute] int smellId, [FromRoute] int severityId)
        {
            var result = _annotationSchemaService.DeleteSeverityFromCodeSmell(smellId, severityId);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpPut]
        [Route("{id}/severities")]
        public IActionResult UpdateSeverityInCodeSmell([FromRoute] int id, [FromBody] SeverityDefinitionDTO severityDTO)
        {
            var severity = _mapper.Map<SeverityDefinition>(severityDTO);
            var result = _annotationSchemaService.UpdateSeverityInCodeSmell(id, severity);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }
    }
}
