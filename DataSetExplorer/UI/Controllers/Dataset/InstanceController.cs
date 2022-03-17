using DataSetExplorer.Core.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace DataSetExplorer.UI.Controllers.Dataset
{
    [Route("api/instances/")]
    [ApiController]
    public class InstanceController : ControllerBase
    {
        private readonly IAnnotationService _annotationService;

        public InstanceController(IAnnotationService annotationService)
        {
            _annotationService = annotationService;
        }

        [HttpGet]
        [Route("{id}/related-instances")]
        public IActionResult GetInstanceWithRelatedInstances([FromRoute] int id)
        {
            var result = _annotationService.GetInstanceWithRelatedInstances(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpGet]
        [Route("{id}/annotations")]
        public IActionResult GetInstanceWithAnnotations([FromRoute] int id)
        {
            var result = _annotationService.GetInstanceWithAnnotations(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }
    }
}