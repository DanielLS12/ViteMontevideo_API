using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViteMontevideo_API.Services.Enums;

namespace ViteMontevideo_API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ConstantesController : ControllerBase
    {
        [HttpGet("TipoPago")]
        public IActionResult ListarTiposPago()
        {
            var data = Enum.GetNames(typeof(TipoPago)).ToList();
            return Ok(data);
        }

        [HttpGet("TipoComercioAdicional")]
        public IActionResult ListarTiposComerciosAdicionales()
        {
            var data = Enum.GetNames(typeof(TipoComercioAdicional)).ToList();
            return Ok(data);
        }

        [HttpGet("Turno")]
        public IActionResult ListarTurnos()
        {
            var data = Enum.GetNames(typeof(Turno)).ToList();
            return Ok(data);
        }
    }
}
