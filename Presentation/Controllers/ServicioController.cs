using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViteMontevideo_API.Presentation.ActionFilters;
using ViteMontevideo_API.Services.Dtos.Servicios.Parameters;
using ViteMontevideo_API.Services.Dtos.Servicios.Requests;
using ViteMontevideo_API.Services.Dtos.Servicios.Responses;
using ViteMontevideo_API.Services.Interfaces;

namespace ViteMontevideo_API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ServicioController : ControllerBase
    {
        private readonly IServicioService _service;

        public ServicioController(IServicioService service)
        {
            _service = service;
        }

        [HttpGet("{idCajaChica}")]
        public async Task<IActionResult> GetAll(int idCajaChica)
        {
            var response = await _service.GetAll(idCajaChica);
            return Ok(response);
        }

        [HttpGet("entradas-vehiculares")]
        public async Task<IActionResult> GetAllEntradasVehiculares()
        {
            var response = await _service.GetAllServiciosEntrada();
            return Ok(response);
        }

        [HttpGet("salidas-vehiculares")]
        public async Task<IActionResult> GetAllSalidasVehiculares([FromQuery] FiltroServicioSalida filtro)
        {
            var response = await _service.GetAllServiciosSalida(filtro);
            return Ok(response);
        }

        [HttpGet("{placa}/entrada-vehicular")]
        public async Task<IActionResult> GetServicioEntradaByPlaca(string placa)
        {
            var response = await _service.GetServicioEntradaByPlaca(placa);
            return Ok(response);
        }

        [HttpGet("{id}/salida-vehicular")]
        public async Task<IActionResult> GetServicioSalidaById(int id)
        {
            var response = await _service.GetServicioSalidaById(id);
            return Ok(response);
        }

        [HttpGet("{placa}/generar-monto")]
        public async Task<IActionResult> GetServicioAmount(string placa)
        {
            var response = await _service.GetServicioAmount(placa);
            return Ok(response);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Insert(ServicioCrearRequestDto request)
        {
            var response = await _service.Insert(request);
            return CreatedAtAction(nameof(GetServicioEntradaByPlaca), new { placa = response.Data is ServicioEntradaResponseDto servicio ? servicio.Vehiculo.Placa : "" }, response);
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Update(int id, [FromBody] ServicioActualizarRequestDto request)
        {
            var response = await _service.Update(id, request);
            return Ok(response);
        }

        [HttpPatch("{placa}/pagar")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Pay(string placa, [FromBody] ServicioPagarRequestDto request)
        {
            var response = await _service.Pay(placa.ToUpper(), request);
            return Ok(response);
        }

        [HttpPatch("{id}/anular-pago")]
        public async Task<IActionResult> CancelPayment(int id)
        {
            var response = await _service.CancelPayment(id);
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
