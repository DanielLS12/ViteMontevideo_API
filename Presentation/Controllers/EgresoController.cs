using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViteMontevideo_API.ActionFilters;
using ViteMontevideo_API.Presentation.Dtos.Egresos;
using ViteMontevideo_API.Presentation.Dtos.Egresos.Filtros;
using ViteMontevideo_API.Services.Interfaces;

namespace ViteMontevideo_API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EgresoController : ControllerBase
    {
        private readonly IEgresoService _service;

        public EgresoController(IEgresoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPageCursor([FromQuery] FiltroEgreso filtro)
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

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Insert(EgresoCrearRequestDto request)
        {
            var response = await _service.Insert(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Data is EgresoResponseDto egreso ? egreso.IdEgreso : 0}, response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] EgresoActualizarRequestDto egresoDto)
        {
            var response = await _service.Update(id, egresoDto);
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
