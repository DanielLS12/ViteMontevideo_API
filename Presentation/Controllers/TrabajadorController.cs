using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViteMontevideo_API.ActionFilters;
using ViteMontevideo_API.Presentation.Dtos.Trabajadores;
using ViteMontevideo_API.Services.Interfaces;

namespace ViteMontevideo_API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TrabajadorController : ControllerBase
    {
        private readonly ITrabajadorService _service;

        public TrabajadorController(ITrabajadorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAll();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(short id)
        {
            var response = await _service.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Insert(TrabajadorCrearRequestDto request)
        {
            var response = await _service.Insert(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Data is TrabajadorResponseDto trabajador ? trabajador.IdTrabajador : 0}, response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Update([FromRoute] short id, [FromBody] TrabajadorActualizarRequestDto request)
        {
            var response = await _service.Update(id, request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(short id)
        {
            var response = await _service.DeleteById(id);
            return Ok(response);
        }
    }
}
