using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.Controllers.Learners.DTOs;
using SmartTutor.LearnerModel;
using SmartTutor.LearnerModel.Exceptions;
using SmartTutor.LearnerModel.Learners;
using SmartTutor.Security.IAM;
using System;
using System.Threading.Tasks;

namespace SmartTutor.Controllers.Learners
{
    [Route("api/learners/")]
    [ApiController]
    public class LearnerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILearnerService _learnerService;
        private readonly IAuthProvider _authProvider;

        public LearnerController(IMapper mapper, ILearnerService learnerService, IAuthProvider authProvider)
        {
            _mapper = mapper;
            _learnerService = learnerService;
            _authProvider = authProvider;
        }

        [HttpPost("register")]
        public async Task<ActionResult<LearnerDTO>> Register([FromBody] LearnerDTO learnerDto)
        {
            var learner = _mapper.Map<Learner>(learnerDto);

            if (bool.Parse(Environment.GetEnvironmentVariable("KEYCLOAK_ON") ?? "false"))
            {
                learner = await _authProvider.Register(learner);
            }

            var registeredLearner = _learnerService.Register(learner);
            return Ok(_mapper.Map<LearnerDTO>(registeredLearner));
        }

        [HttpPost("login")]
        public ActionResult<LearnerDTO> Login([FromBody] LoginDTO login)
        {
            try
            {
                var learner = _learnerService.Login(login.StudentIndex);
                return Ok(_mapper.Map<LearnerDTO>(learner));
            }
            catch (LearnerWithStudentIndexNotFound e)
            {
                return NotFound(e.Message);
            }
        }
    }
}