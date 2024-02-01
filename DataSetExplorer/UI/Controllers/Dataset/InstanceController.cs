using CodeModel;
using CodeModel.Serialization;
using DataSetExplorer.Core.DataSets;
using Microsoft.AspNetCore.Mvc;

namespace DataSetExplorer.UI.Controllers.Dataset
{
    [Route("api/instances/")]
    [ApiController]
    public class InstanceController : ControllerBase
    {
        private readonly IInstanceService _instanceService;

        public InstanceController(IInstanceService instanceService)
        {
            _instanceService = instanceService;
        }

        [HttpGet]
        [Route("{id}/related-instances")]
        public IActionResult GetInstanceWithRelatedInstances([FromRoute] int id)
        {
            var result = _instanceService.GetInstanceWithRelatedInstances(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpGet]
        [Route("{id}/annotations")]
        public IActionResult GetInstanceWithAnnotations([FromRoute] int id)
        {
            var result = _instanceService.GetInstanceWithAnnotations(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpGet]
        [Route("{id}/class-cohesion-graph")]
        public IActionResult GetCohesionGraphForInstance([FromRoute] int id)
        {
            var result = _instanceService.GetInstanceWithAnnotations(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            var LoadedFile = _instanceService.GetFileFromGit(result.Value.Link);
            var exporter = new ClassCohesionGraphExporter();
            var project = new CodeModelFactory().CreateProject(new []{ LoadedFile });
            var actualClass = project.Classes;
            return Ok(exporter.GetJSON(actualClass[0]));
        }
    }
}