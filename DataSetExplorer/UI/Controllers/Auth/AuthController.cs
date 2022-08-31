using AutoMapper;
using DataSetExplorer.Core.Annotations.Model;
using DataSetExplorer.Core.Auth;
using DataSetExplorer.UI.Controllers.Auth.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DataSetExplorer.UI.Controllers.Auth
{
    [Route("api/auth/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public AuthController(IMapper mapper, IAuthService authService)
        {
            _mapper = mapper;
            _authService = authService;
        }

        [HttpPost]
        public IActionResult RegisterAnnotator([FromBody] AnnotatorDTO annotatorDTO)
        {
            var annotator = _authService.Save(_mapper.Map<Annotator>(annotatorDTO));
            return Ok(annotator);
        }

        [HttpGet]
        [Route("email/{email}")]
        public IActionResult GetAnnotatorByEmail([FromRoute] string email)
        {
            return Ok(_authService.GetAnnotatorByEmail(email).Value);
        }

        [HttpGet]
        [Route("id/{id}")]
        public IActionResult GetAnnotatorById([FromRoute] int id)
        {
            return Ok(_authService.GetAnnotatorById(id).Value);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateAnnotator([FromRoute] int id, [FromBody] AnnotatorDTO annotatorDTO)
        {
            var annotator = _mapper.Map<Annotator>(annotatorDTO);
            annotator.Id = id;
            return Ok(_authService.Save(annotator).Value);
        }
    }
}
