﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViteMontevideo_API.ActionFilters;
using ViteMontevideo_API.Presentation.Dtos.ComerciosAdicionales;
using ViteMontevideo_API.Presentation.Dtos.ComerciosAdicionales.Filtros;
using ViteMontevideo_API.Services.Interfaces;

namespace ViteMontevideo_API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ComercioAdicionalController : ControllerBase
    {
        private readonly IComercioAdicionalService _service;

        public ComercioAdicionalController(IComercioAdicionalService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPageCursor([FromQuery] FiltroComercioAdicional filtro)
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
        public async Task<IActionResult> Insert(ComercioAdicionalCrearRequestDto request)
        {
            var response = await _service.Insert(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Data is ComercioAdicionalResponseDto comercioAdicional ? comercioAdicional.IdComercioAdicional : 0}, response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ComercioAdicionalActualizarRequestDto request)
        {
            var response = await _service.Update(id, request);
            return Ok(response);
        }

        [HttpPatch("{id}/pagar")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Pay([FromRoute] int id, [FromBody] ComercioAdicionalPagarRequestDto request)
        {
            var response = await _service.Pay(id, request);
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
