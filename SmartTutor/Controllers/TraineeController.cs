using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.ContentModel;
using SmartTutor.ContentModel.ProgressModel;
using SmartTutor.Controllers.DTOs.Trainee;
using SmartTutor.Exceptions;

namespace SmartTutor.Controllers
{
    [Route("api/trainees/")]
    [ApiController]
    public class TraineeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITraineeService _traineeService;

        public TraineeController(IMapper mapper, ITraineeService traineeService)
        {
            _mapper = mapper;
            _traineeService = traineeService;
        }

        [HttpPost("")]
        public ActionResult SubmitQuestionAnswers([FromBody] TraineeDTO trainee)
        {
            try
            {
                _traineeService.RegisterTrainee(_mapper.Map<Trainee>(trainee));
            }
            catch (TraineeWithStudentIndexAlreadyExists)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}