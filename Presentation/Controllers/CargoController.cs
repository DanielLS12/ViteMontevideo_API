using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViteMontevideo_API.Presentation.ActionFilters;
using ViteMontevideo_API.Services.Dtos.Cargos.Requests;
using ViteMontevideo_API.Services.Interfaces;

namespace ViteMontevideo_API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class CargoController : ControllerBase
    {
        public readonly ICargoService _service;

        public CargoController(ICargoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAll();
            return Ok(response);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Insert(CargoRequestDto request)
        {
            var response = await _service.Insert(request);
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Update([FromRoute] byte id, [FromBody] CargoRequestDto request)
        {
            var response = await _service.Update(id, request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(byte id)
        {
            var response = await _service.DeleteById(id);
            return Ok(response);
        }
    }
}
