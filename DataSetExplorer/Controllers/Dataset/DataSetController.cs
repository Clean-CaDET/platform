using AutoMapper;
using DataSetExplorer.Controllers.Dataset.DTOs;
using DataSetExplorer.DataSetBuilder.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DataSetExplorer.Controllers.Dataset
{
    [Route("api/dataset/")]
    [ApiController]
    public class DataSetController : ControllerBase
    {
        private readonly string _gitClonePath;

        private readonly IMapper _mapper;
        private readonly IDataSetCreationService _dataSetCreationService;

        public DataSetController(IMapper mapper, IDataSetCreationService creationService, IConfiguration configuration)
        {
            _mapper = mapper;
            _dataSetCreationService = creationService;
            _gitClonePath = configuration.GetValue<string>("Workspace:GitClonePath");
        }

        [HttpPost]
        [Route("{name}")]
        public IActionResult CreateDataSet([FromBody] List<CodeSmellDTO> codeSmells, [FromRoute] string name)
        {
            var smells = new List<CodeSmell>();
            foreach (var codeSmell in codeSmells) smells.Add(_mapper.Map<CodeSmell>(codeSmell));

            var result = _dataSetCreationService.CreateEmptyDataSet(name, smells);
            return Ok(result.Value);
        }

        [HttpPost]
        [Route("{id}/add-projects")]
        public IActionResult CreateDataSetProject([FromBody] List<ProjectDTO> projects, [FromRoute] int id)
        {
            var dataSetProjects = new List<DataSetProject>();
            foreach (var project in projects) dataSetProjects.Add(_mapper.Map<DataSetProject>(project));

            var result = _dataSetCreationService.AddProjectsToDataSet(id, _gitClonePath, dataSetProjects);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Accepted(result.Value);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetDataSet([FromRoute] int id)
        {
            var result = _dataSetCreationService.GetDataSet(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpGet]
        public IActionResult GetAllDataSets()
        {
            var result = _dataSetCreationService.GetAllDataSets();
            return Ok(result.Value);
        }

        [HttpGet]
        [Route("project/{id}")]
        public IActionResult GetDataSetProject([FromRoute] int id)
        {
            var result = _dataSetCreationService.GetDataSetProject(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }
    }
}
