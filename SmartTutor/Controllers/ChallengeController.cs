using Microsoft.AspNetCore.Mvc;
using RepositoryCompiler.CodeModel.CaDETModel.CodeItems;
using RepositoryCompiler.Controllers;
using SmartTutor.Controllers.DTOs.Challenge;
using System.Collections.Generic;

namespace SmartTutor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengeController : ControllerBase
    {
        [HttpPost("check")]
        public ChallengeCheckResponseDTO CheckChallengeCompletion([FromBody] ChallengeCheckRequestDTO checkRequestDto)
        {
            var text = checkRequestDto.ChallengeId == 1 ? "Fail." : "Success.";

            // TODO: This will be moved to challenge service
            List<CaDETClass> caDETClasses = new CodeRepositoryService().BuildClassesModel(checkRequestDto.SourceCode);

            return new ChallengeCheckResponseDTO(text);
        }
    }
}
