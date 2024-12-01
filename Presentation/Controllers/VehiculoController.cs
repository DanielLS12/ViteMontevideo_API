using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViteMontevideo_API.Presentation.ActionFilters;
using ViteMontevideo_API.Services.Dtos.Vehiculos.Parameters;
using ViteMontevideo_API.Services.Dtos.Vehiculos.Requests;
using ViteMontevideo_API.Services.Dtos.Vehiculos.Responses;
using ViteMontevideo_API.Services.Interfaces;

namespace ViteMontevideo_API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class VehiculoController : ControllerBase
    {
        private readonly IVehiculoService _service;

        public VehiculoController(IVehiculoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPageCursor([FromQuery] FiltroVehiculo filtro)
        {
            var response = await _service.GetAllPageCursor(filtro);
            Response.Headers.Add("X-Pagination", $"Next Cursor={response.SiguienteCursor}");
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _service.GetById(id);
            return Ok(response);
        }

        [HttpGet("{placa}")]
        public async Task<IActionResult> GetByPlaca(string placa)
        {
            var response = await _service.GetByPlaca(placa);
            return Ok(response);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Insert(VehiculoCrearRequestDto request)
        {
            var response = await _service.Insert(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Data is VehiculoFullResponseDto vehiculo ? vehiculo.IdVehiculo : 0 }, response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Update(int id, [FromBody] VehiculoActualizarRequestDto request)
        {
            var response = await _service.Update(id, request);
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
