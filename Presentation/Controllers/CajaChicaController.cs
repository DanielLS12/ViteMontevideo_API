using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViteMontevideo_API.Presentation.ActionFilters;
using ViteMontevideo_API.Services.Dtos.CajasChicas.Parameters;
using ViteMontevideo_API.Services.Dtos.CajasChicas.Requests;
using ViteMontevideo_API.Services.Dtos.CajasChicas.Responses;
using ViteMontevideo_API.Services.Interfaces;

namespace ViteMontevideo_API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CajaChicaController : ControllerBase
    {
        private readonly ICajaChicaService _service;

        public CajaChicaController(ICajaChicaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPageCursor([FromQuery] FiltroCajaChica filtro)
        {
            var response = await _service.GetAllPageCursor(filtro);
            Response.Headers.Add("X-Pagination", $"Next Cursor={response.SiguienteCursor}");
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _service.GetById(id);
            return Ok(response);
        }

        [HttpGet("{fecha}/informe")]
        public async Task<IActionResult> GetAllInformes(DateTime fecha)
        {
            var response = await _service.GetAllInformes(fecha);
            return Ok(response);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Insert(CajaChicaCrearRequestDto request)
        {
            var response = await _service.Insert(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Data is CajaChicaResponseDto cajaChica ? cajaChica.IdCaja : 0}, response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CajaChicaActualizarRequestDto request)
        {
            var response = await _service.Update(id, request);
            return Ok(response);
        }

        [HttpPatch("{id}/abrir")]
        public async Task<IActionResult> Open(int id)
        {
            var response = await _service.Open(id);
            return Ok(response);
        }

        [HttpPatch("{id}/cerrar")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Close([FromRoute] int id, [FromBody] CajaChicaCerrarRequestDto request)
        {
            var response = await _service.Close(id, request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var response = await _service.DeleteById(id);
            return Ok(response);
        }
    }
}
