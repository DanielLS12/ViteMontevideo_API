using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using ViteMontevideo_API.Presentation.ActionFilters;
using ViteMontevideo_API.Services.Dtos.Usuarios;
using ViteMontevideo_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using ViteMontevideo_API.Configuration;

namespace ViteMontevideo_API.Presentation.Controllers
{
    [EnableCors("ReglasCors")]
    [EnableRateLimiting(nameof(RateLimitPolicy.HighFrequencyPolicy))]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAvailableUsers()
        {
            var response = await _usuarioService.GetAvailableUsers();
            return Ok(response);
        }

        [HttpGet("{username}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            var response = await _usuarioService.GetByUsername(username);
            return Ok(response);
        }

        //[HttpPost("Registrar")]
        //[ServiceFilter(typeof(ValidationFilterAttribute))]
        //public async Task<IActionResult> Registrar(UsuarioRequestDto request)
        //{
        //    var response = await _usuarioService.Register(request);
        //    return CreatedAtAction(nameof(GetByUsername), new { username = response.Data is UsuarioResponseDto usuario ? usuario.Nombre : "" }, response);
        //}

        [HttpPost("Login")]
        [EnableRateLimiting(nameof(RateLimitPolicy.LowFrequencyPolicy))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Login(UsuarioRequestDto request)
        {
            var response = await _usuarioService.Login(request);
            return Ok(response);
        }
    }
}
