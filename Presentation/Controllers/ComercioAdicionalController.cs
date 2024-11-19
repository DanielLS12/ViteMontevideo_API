using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.ActionFilters;
using ViteMontevideo_API.Exceptions;
using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Presentation.Dtos.ComerciosAdicionales;
using ViteMontevideo_API.Presentation.Dtos.ComerciosAdicionales.Filtros;
using ViteMontevideo_API.Presentation.Dtos.Common;
using ViteMontevideo_API.Presentation.Dtos.Cursor;

namespace ViteMontevideo_API.Presentation.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ComercioAdicionalController : ControllerBase
    {
        private readonly EstacionamientoContext _dbContext;
        private readonly IMapper _mapper;

        public ComercioAdicionalController(EstacionamientoContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Listar([FromQuery] FiltroComercioAdicional filtro, [FromQuery] CursorParams parametros)
        {
            const int MaxRegistros = 50;
            var query = _dbContext.ComerciosAdicionales.Include(cad => cad.Cliente).AsQueryable();

            // Filtraje
            if (!string.IsNullOrWhiteSpace(filtro.Cliente))
            {
                var terminos = filtro.Cliente.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (var termino in terminos)
                {
                    var lowerTermino = termino.ToLower();
                    query = query.Where(cad =>
                        EF.Functions.Like(cad.Cliente.Nombres.ToLower(), $"%{lowerTermino}%") ||
                        EF.Functions.Like(cad.Cliente.Apellidos.ToLower(), $"%{lowerTermino}%"));
                }
            }

            if (filtro.Tipo.HasValue)
            {
                query = query.Where(cad => cad.TipoComercioAdicional == filtro.Tipo.Value.ToString());
            }

            if (filtro.EstaPagado.HasValue)
            {
                if (filtro.EstaPagado.Value)
                {
                    if (filtro.TipoPago.HasValue)
                    {
                        query = query.Where(cad => cad.TipoPago == filtro.TipoPago.Value.ToString());
                    }
                    query = query.Where(cad => cad.IdCaja != null);
                }
                else
                {
                    query = query.Where(cad => cad.IdCaja == null);
                }
            }

            int cantidad = query.Count();
            decimal totalMonto = query.Sum(ca => ca.Monto);

            // Cursor
            if (parametros.Cursor > 0)
                query = query.Where(v => v.IdCliente < parametros.Cursor);

            query = query.Take(parametros.Count > MaxRegistros ? MaxRegistros : parametros.Count);

            // Listar
            var data = query
                .AsNoTracking()
                .OrderByDescending(c => c.IdComercioAdicional)
                .ProjectTo<ComercioAdicionalResponseDto>(_mapper.ConfigurationProvider)
                .ToList();

            var siguiente = data.Any() ? data.LastOrDefault()?.IdComercioAdicional : 0;

            if (siguiente == 0)
                cantidad = 0;

            Response.Headers.Add("X-Pagination", $"Next Cursor={siguiente}");

            return Ok(new { cantidad, totalMonto, siguiente, data });
        }

        [HttpGet("{id}")]
        public IActionResult Obtener(int id)
        {
            var comercioAdicional = _dbContext.ComerciosAdicionales
                .AsNoTracking()
                .ProjectTo<ComercioAdicionalResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefault(ca => ca.IdComercioAdicional == id) ?? throw new NotFoundException("Comercio adicional no encontrado.");

            return Ok(comercioAdicional);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Guardar(ComercioAdicionalCrearRequestDto comercioAdicionalDto)
        {
            var cliente = _dbContext.Clientes.Find(comercioAdicionalDto.IdCliente) ?? throw new NotFoundException("Cliente no encontrado.");

            var comercioAdicional = _mapper.Map<ComercioAdicional>(comercioAdicionalDto);

            _dbContext.ComerciosAdicionales.Add(comercioAdicional);
            _dbContext.SaveChanges();

            var response = ApiResponse.Success("El comercio adicional ha sido agregado.");

            return CreatedAtAction(nameof(Obtener), new { id = comercioAdicional.IdComercioAdicional }, response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Editar([FromRoute] int id, [FromBody] ComercioAdicionalActualizarRequestDto comercioAdicionalDto)
        {
            var dbComercioAdicional = _dbContext.ComerciosAdicionales.Find(id) ?? throw new NotFoundException("Ingreso adicional no encontrado.");

            if (comercioAdicionalDto.IdCliente != null)
            {
                var cliente = _dbContext.Clientes.Find(comercioAdicionalDto.IdCliente) ?? throw new NotFoundException("Cliente no encontrado.");
                dbComercioAdicional.IdCliente = cliente.IdCliente;
            }

            dbComercioAdicional.TipoComercioAdicional = comercioAdicionalDto.TipoComercioAdicional ?? dbComercioAdicional.TipoComercioAdicional;
            dbComercioAdicional.Monto = comercioAdicionalDto.Monto ?? dbComercioAdicional.Monto;
            dbComercioAdicional.Observacion = string.IsNullOrWhiteSpace(comercioAdicionalDto.Observacion) ? null : comercioAdicionalDto.Observacion.Trim();

            _dbContext.SaveChanges();

            var response = ApiResponse.Success("El comercio adicional ha sido actualizado.");

            return Ok(response);
        }

        [HttpPatch("{id}/pagar")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Pagar([FromRoute] int id, [FromBody] ComercioAdicionalPagarRequestDto comercioAdicionalDto)
        {
            var comercioAdicional = _dbContext.ComerciosAdicionales.Find(id) ?? throw new NotFoundException("Comercio adicional no encontrado.");

            if (comercioAdicional.FechaPago != null)
                throw new BadRequestException("Este comercio adicional ya estaba pagado.");

            var cajaChicaAbierta = _dbContext.CajasChicas.FirstOrDefault(cc => cc.Estado == true) ?? throw new NotFoundException("No hay caja chica abierta.");

            comercioAdicional.IdCaja = cajaChicaAbierta.IdCaja;
            comercioAdicional.FechaPago = comercioAdicionalDto.FechaPago;
            comercioAdicional.HoraPago = comercioAdicionalDto.HoraPago;
            comercioAdicional.TipoPago = comercioAdicionalDto.TipoPago;

            _dbContext.SaveChanges();

            var response = ApiResponse.Success("El comercio adicional ha sido pagado.");

            return Ok(response);
        }

        [HttpPatch("{id}/anular-pago")]
        public IActionResult AnularPago(int id)
        {
            var comercioAdicional = _dbContext.ComerciosAdicionales.Find(id) ?? throw new NotFoundException("Comercio adicional no encontrado.");

            if (comercioAdicional.IdCaja == null)
                throw new BadRequestException("Este comercio adicional ya estaba anulado.");

            comercioAdicional.IdCaja = null;
            comercioAdicional.FechaPago = null;
            comercioAdicional.HoraPago = null;
            comercioAdicional.TipoPago = null;

            _dbContext.SaveChanges();

            var response = ApiResponse.Success("El pago del comercio adicional ha sido anulado.");

            return Ok(response);
        }


        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            var comercioAdicional = _dbContext.ComerciosAdicionales.Find(id) ?? throw new NotFoundException("Comercio adicional no encontrado.");

            _dbContext.ComerciosAdicionales.Remove(comercioAdicional);
            _dbContext.SaveChanges();

            var response = ApiResponse.Success("El comercio adicional ha sido eliminado.");

            return Ok(response);
        }
    }
}
