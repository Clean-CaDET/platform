using DataSetExplorer.Controllers.Dataset.DTOs;
using DataSetExplorer.DataSetBuilder.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSetExplorer.Controllers.Dataset
{
    [Route("api/dataset/")]
    [ApiController]
    public class DataSetController : ControllerBase
    {
        private readonly string _cloneGitPath = "../../ClonedProjects/";

        private readonly IDataSetCreationService _dataSetCreationService;
        private readonly IDataSetAnalysisService _dataSetAnalysisService;

        public DataSetController(IDataSetCreationService creationService, IDataSetAnalysisService analysisService)
        {
            _dataSetCreationService = creationService;
            _dataSetAnalysisService = analysisService;
        }

        [HttpPost]
        public IActionResult CreateDataSet([FromBody] List<ProjectDTO> projects)
        {
            var dataSets = new List<DataSet>();
            foreach (var project in projects)
            {
                dataSets.Add(_dataSetCreationService.CreateDataSet(_cloneGitPath, project.Name, project.Url));
            }
            
            return Ok(dataSets);
        }

        [HttpGet]
        [Route("requiring-additional-annotation/{dataSetId}")]
        public IActionResult FindInstancesRequiringAdditionalAnnotation(int dataSetId)
        {
            try
            {
                return Ok(_dataSetAnalysisService.FindInstancesRequiringAdditionalAnnotation(dataSetId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("disagreeing-annotations/{dataSetId}")]
        public IActionResult FindInstancesWithAllDisagreeingAnnotations(int dataSetId)
        {
            try
            {
                return Ok(_dataSetAnalysisService.FindInstancesWithAllDisagreeingAnnotations(dataSetId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
