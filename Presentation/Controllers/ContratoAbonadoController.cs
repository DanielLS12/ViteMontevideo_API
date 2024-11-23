using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViteMontevideo_API.ActionFilters;
using ViteMontevideo_API.Presentation.Dtos.ContratosAbonado;
using ViteMontevideo_API.Presentation.Dtos.ContratosAbonado.Filtros;
using ViteMontevideo_API.Services.Interfaces;

namespace ViteMontevideo_API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ContratoAbonadoController : ControllerBase
    {
        private readonly IContratoAbonadoService _service;

        public ContratoAbonadoController(IContratoAbonadoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPageCursor([FromQuery] FiltroContratoAbonado filtro)
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
        public async Task<IActionResult> Insert(ContratoAbonadoCrearRequestDto request)
        {
            var response = await _service.Insert(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Data is ContratoAbonadoResponseDto abonado ? abonado.IdContratoAbonado : 0}, response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ContratoAbonadoActualizarRequestDto request)
        {
            var response = await _service.Update(id,request);
            return Ok(response);
        }

        [HttpPatch("{id}/pagar")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Pay([FromRoute] int id, [FromBody] ContratoAbonadoPagarRequestDto request)
        {
            var response = await _service.Pay(id,request);
            return Ok(response);
        }

        [HttpPatch("{id}/anular-pago")]
        public async Task<IActionResult> CancelPayment([FromRoute] int id)
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
