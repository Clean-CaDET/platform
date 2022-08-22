using System.Collections.Generic;
using AutoMapper;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.DataSets;
using DataSetExplorer.Core.DataSets.Model;
using DataSetExplorer.Core.DataSetSerializer;
using DataSetExplorer.UI.Controllers.Dataset.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DataSetExplorer.UI.Controllers.Dataset
{
    [Route("api/datasets/")]
    [ApiController]
    public class DataSetController : ControllerBase
    {
        private readonly string _gitClonePath;

        private readonly IMapper _mapper;
        private readonly IDataSetCreationService _dataSetCreationService;
        private readonly IDataSetExportationService _dataSetExportationService;

        public DataSetController(IMapper mapper, IDataSetCreationService creationService, IConfiguration configuration,
            IDataSetExportationService exportationService)
        {
            _mapper = mapper;
            _dataSetCreationService = creationService;
            _dataSetExportationService = exportationService;
            _gitClonePath = configuration.GetValue<string>("Workspace:GitClonePath");
        }

        [HttpPut]
        public IActionResult UpdateDataSet([FromBody] DatasetDTO datasetDto)
        {
            var dataset = _mapper.Map<DataSet>(datasetDto);
            var result = _dataSetCreationService.UpdateDataSet(dataset);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }

        [HttpPost]
        [Route("export")]
        public IActionResult ExportDataSet([FromBody] DraftDataSetExportDTO dataSetDTO)
        {
            var exportPath = _dataSetExportationService.ExportDraft(dataSetDTO);
            return Ok(new FluentResults.Result().WithSuccess("Successfully exported to " + exportPath));
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
        [Route("{id}/projects")]
        public IActionResult CreateDataSetProject([FromBody] ProjectCreationDTO data, [FromRoute] int id)
        {
            var dataSetProject = _mapper.Map<DataSetProject>(data.Project);
            var smellFilters = _mapper.Map<List<SmellFilter>>(data.SmellFilters);

            var result = _dataSetCreationService.AddProjectToDataSet(id, _gitClonePath, dataSetProject, smellFilters, data.BuildSettings);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Accepted(result.Value);
        }
        
        [HttpGet]
        [Route("{id}/code-smells")]
        public IActionResult GetDataSetCodeSmells([FromRoute] int id)
        {
            var result = _dataSetCreationService.GetDataSetCodeSmells(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
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
        
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteDataSet([FromRoute] int id)
        {
            var result = _dataSetCreationService.DeleteDataSet(id);
            if (result.IsFailed) return BadRequest(new { message = result.Reasons[0].Message });
            return Ok(result.Value);
        }
    }
}
