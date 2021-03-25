using Microsoft.AspNetCore.Mvc;
using SmartTutor.ContentModel;
using SmartTutor.Controllers.DTOs.Challenge;

namespace SmartTutor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengeController : ControllerBase
    {
        private readonly IChallengeService _challengeService;

        public ChallengeController(IChallengeService challengeService)
        {
            _challengeService = challengeService;
        }

        [HttpPost("check")]
        public ChallengeCheckResponseDTO CheckChallengeCompletion([FromBody] ChallengeCheckRequestDTO checkRequestDto)
        {
            bool completed = _challengeService.CheckSubmittedChallengeCompletion(checkRequestDto.SourceCode, checkRequestDto.ChallengeId);
            var text = completed ? "Success." : "Fail.";
            return new ChallengeCheckResponseDTO(text);
        }
    }
}
