using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViteMontevideo_API.Presentation.ActionFilters;
using ViteMontevideo_API.Services.Dtos.Categorias.Requests;
using ViteMontevideo_API.Services.Dtos.Categorias.Responses;
using ViteMontevideo_API.Services.Interfaces;

namespace ViteMontevideo_API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _service;

        public CategoriaController(ICategoriaService service)
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
        public async Task<IActionResult> Insert(CategoriaRequestDto categoria)
        {
            var response = await _service.Insert(categoria);
            return CreatedAtAction(nameof(GetById), new { id = response.Data is CategoriaResponseDto categoriaResponse ? categoriaResponse.IdCategoria : 0 }, response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Update([FromRoute] short id, [FromBody] CategoriaRequestDto categoria)
        {
            var response = await _service.Update(id,categoria);
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
