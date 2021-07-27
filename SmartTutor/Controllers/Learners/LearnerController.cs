using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartTutor.Controllers.Learners.DTOs;
using SmartTutor.Keycloak;
using SmartTutor.LearnerModel;
using SmartTutor.LearnerModel.Exceptions;
using SmartTutor.LearnerModel.Learners;

namespace SmartTutor.Controllers.Learners
{
    [Route("api/learners/")]
    [ApiController]
    public class LearnerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILearnerService _learnerService;
        private readonly IKeycloakService _keycloakService;

        public LearnerController(IMapper mapper, ILearnerService learnerService, IKeycloakService keycloakService)
        {
            _mapper = mapper;
            _learnerService = learnerService;
            _keycloakService = keycloakService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<LearnerDTO>> Register([FromBody] LearnerDTO learnerDto)
        {
            //TODO: Check if Keycloak is on before calling this method.
            var learner = await _keycloakService.Register(_mapper.Map<Learner>(learnerDto));
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