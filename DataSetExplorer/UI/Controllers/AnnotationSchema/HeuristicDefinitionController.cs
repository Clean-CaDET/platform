using AutoMapper;
using DataSetExplorer.Core.AnnotationSchema;
using DataSetExplorer.Core.AnnotationSchema.Model;
using DataSetExplorer.UI.Controllers.AnnotationSchema.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DataSetExplorer.UI.Controllers.AnnotationSchema
{
    [Route("api/annotation-schema/heuristics/")]
    [ApiController]
    public class HeuristicDefinitionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAnnotationSchemaService _annotationSchemaService;

        public HeuristicDefinitionController(IMapper mapper, IAnnotationSchemaService annotationSchemaService)
        {
            _mapper = mapper;
            _annotationSchemaService = annotationSchemaService;
        }
        
        [HttpGet]
        public IActionResult GetAllHeuristics()
        {
            var result = _annotationSchemaService.GetAllHeuristics();
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpPost]
        public IActionResult CreateHeuristic([FromBody] HeuristicDefinitionDTO heuristicDto)
        {
            var heuristic = _mapper.Map<HeuristicDefinition>(heuristicDto);
            var result = _annotationSchemaService.CreateHeuristic(heuristic);
            if (result.IsFailed) return NotFound(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateHeuristic([FromRoute] int id, [FromBody] HeuristicDefinitionDTO heuristicDto)
        {
            var heuristic = _mapper.Map<HeuristicDefinition>(heuristicDto);
            var result = _annotationSchemaService.UpdateHeuristic(id, heuristic);
            if (result.IsFailed) return NotFound(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteHeuristic([FromRoute] int id)
        {
            var result = _annotationSchemaService.DeleteHeuristic(id);
            if (result.IsFailed) return BadRequest(new {message = result.Reasons[0].Message});
            return Ok(result.Value);
        }
    }
}
