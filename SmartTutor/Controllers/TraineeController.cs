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

        [HttpPost("register")]
        public ActionResult<TraineeDTO> RegisterTrainee([FromBody] TraineeDTO trainee)
        {
            var registeredTrainee = _traineeService.RegisterTrainee(_mapper.Map<Trainee>(trainee));
            return Ok(_mapper.Map<TraineeDTO>(registeredTrainee));
        }

        [HttpPost("login")]
        public ActionResult<TraineeDTO> LoginTrainee([FromBody] LoginDTO login)
        {
            try
            {
                var loggedInTrainee = _traineeService.LoginTrainee(login.StudentIndex);
                return Ok(_mapper.Map<TraineeDTO>(loggedInTrainee));
            }
            catch (TraineeWIthStudentIndexNotFound)
            {
                return BadRequest();
            }
        }
    }
}