using AutoMapper;
using DataSetExplorer.Core.DataSets;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.UI.Controllers.Dataset.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DataSetExplorer.UI.Controllers.Dataset
{
    [Route("api/projects/")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly string _gitClonePath;

        private readonly IMapper _mapper;
        private readonly IDataSetCreationService _dataSetCreationService;

        public ProjectController(IMapper mapper, IDataSetCreationService creationService, IConfiguration configuration)
        {
            _mapper = mapper;
            _dataSetCreationService = creationService;
            _gitClonePath = configuration.GetValue<string>("Workspace:GitClonePath");
        }
        
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetDataSetProject([FromRoute] int id)
        {
            var result = _dataSetCreationService.GetDataSetProject(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }
        
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteDataSetProject([FromRoute] int id)
        {
            var result = _dataSetCreationService.DeleteDataSetProject(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }
        
        [HttpPut]
        public IActionResult UpdateDataSetProject([FromBody] ProjectUpdateDTO projectDto)
        {
            var project = _mapper.Map<DataSetProject>(projectDto);
            var result = _dataSetCreationService.UpdateDataSetProject(project);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }
    }
}
