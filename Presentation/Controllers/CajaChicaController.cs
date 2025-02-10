using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ViteMontevideo_API.Configuration;
using ViteMontevideo_API.Presentation.ActionFilters;
using ViteMontevideo_API.Services.Dtos.CajasChicas.Parameters;
using ViteMontevideo_API.Services.Dtos.CajasChicas.Requests;
using ViteMontevideo_API.Services.Dtos.CajasChicas.Responses;
using ViteMontevideo_API.Services.Interfaces;

namespace ViteMontevideo_API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [EnableRateLimiting(nameof(RateLimitPolicy.LowFrequencyPolicy))]
    [Authorize(Roles = "Admin,Cajero")]
    [ApiController]
    public class CajaChicaController : ControllerBase
    {
        private readonly ICajaChicaService _cajaChicaService;
        private readonly IComercioAdicionalService _comercioAdicionalService;
        private readonly IServicioService _servicioService;
        private readonly IContratoAbonadoService _contratoAbonadoService;
        private readonly IEgresoService _egresoService;

        public CajaChicaController(
            ICajaChicaService cajaChicaService,
            IComercioAdicionalService comercioAdicionalService, 
            IServicioService servicioService,
            IContratoAbonadoService contratoAbonadoService,
            IEgresoService egresoService)
        {
            _cajaChicaService = cajaChicaService;
            _comercioAdicionalService = comercioAdicionalService;
            _servicioService = servicioService;
            _contratoAbonadoService = contratoAbonadoService;
            _egresoService = egresoService;
        }

        [HttpGet("{id}/comercios-adicionales")]
        [Authorize]
        public async Task<IActionResult> GetAllComerciosAdicionales(int id)
        {
            var response = await _comercioAdicionalService.GetAll(id);
            return Ok(response);
        }

        [HttpGet("{id}/contratos-abonados")]
        [Authorize]
        public async Task<IActionResult> GetAllContratosAbonados(int id)
        {
            var response = await _contratoAbonadoService.GetAll(id);
            return Ok(response);
        }

        [HttpGet("{id}/servicios")]
        [Authorize]
        public async Task<IActionResult> GetAllServicios(int id)
        {
            var response = await _servicioService.GetAll(id);
            return Ok(response);
        }

        [HttpGet("{id}/egresos")]
        [Authorize]
        public async Task<IActionResult> GetAllEgresos(int id)
        {
            var response = await _egresoService.GetAll(id);
            return Ok(response);
        }

        [HttpGet]
        [EnableRateLimiting(nameof(RateLimitPolicy.HighFrequencyPolicy))]
        [Authorize]
        public async Task<IActionResult> GetAllPageCursor([FromQuery] FiltroCajaChica filtro)
        {
            var response = await _cajaChicaService.GetAllPageCursor(filtro);
            Response.Headers.Append("X-Pagination", $"Next Cursor={response.SiguienteCursor}");
            return Ok(response);
        }

        [HttpGet("{id}")]
        [EnableRateLimiting(nameof(RateLimitPolicy.HighFrequencyPolicy))]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _cajaChicaService.GetById(id);
            return Ok(response);
        }

        [HttpGet("{fecha}/informe")]
        [Authorize]
        public async Task<IActionResult> GetAllInformes(DateTime fecha)
        {
            var response = await _cajaChicaService.GetAllInformes(fecha);
            return Ok(response);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Insert(CajaChicaCrearRequestDto request)
        {
            var response = await _cajaChicaService.Insert(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Data is CajaChicaResponseDto cajaChica ? cajaChica.IdCaja : 0}, response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CajaChicaActualizarRequestDto request)
        {
            var response = await _cajaChicaService.Update(id, request);
            return Ok(response);
        }

        [HttpPatch("{id}/abrir")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Open(int id)
        {
            var response = await _cajaChicaService.Open(id);
            return Ok(response);
        }

        [HttpPatch("{id}/cerrar")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Close([FromRoute] int id, [FromBody] CajaChicaCerrarRequestDto request)
        {
            var response = await _cajaChicaService.Close(id, request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var response = await _cajaChicaService.DeleteById(id);
            return Ok(response);
        }
    }
}
