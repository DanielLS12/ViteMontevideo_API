#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.ActionFilters;
using ViteMontevideo_API.Dtos.Common;
using ViteMontevideo_API.Dtos.ContratosAbonado;
using ViteMontevideo_API.Middleware.Exceptions;
using ViteMontevideo_API.models;

namespace ViteMontevideo_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ContratoAbonadoController : ControllerBase
    {
        private readonly EstacionamientoContext _dbContext;
        private readonly IMapper _mapper;

        public ContratoAbonadoController (EstacionamientoContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Listar()
        {
            var data = _dbContext.ContratosAbonados
                .AsNoTracking()
                .OrderByDescending(c => c.IdContratoAbonado)
                .ProjectTo<ContratoAbonadoResponseDto>(_mapper.ConfigurationProvider)
                .ToList();

            int cantidad = data.Count;

            return Ok(new DataResponse<ContratoAbonadoResponseDto>(cantidad,data));
        }

        [HttpGet("{id}")]
        public IActionResult Obtener(int id)
        {
            var contratoAbonado = _dbContext.ContratosAbonados
                .AsNoTracking()
                .ProjectTo<ContratoAbonadoResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefault(ca => ca.IdContratoAbonado == id) ?? throw new NotFoundException("Abonado no encontrado.");       

            return Ok(contratoAbonado);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Guardar(ContratoAbonadoCrearRequestDto contratoAbonadoDto)
        {
            var buscarVehiculo = _dbContext.Vehiculos.Find(contratoAbonadoDto.IdVehiculo) ?? throw new NotFoundException("Vehículo no encontrado.");

            var tieneServicioEnMarcha = _dbContext.Servicios.Any(s => s.IdVehiculo == contratoAbonadoDto.IdVehiculo && s.EstadoPago == false);

            if (tieneServicioEnMarcha)
                throw new BadRequestException("Este vehículo tiene un servicio en marcha con modalidad de hora o turno. No se pudo crear el abonado.");

            var contratoAbonado = _mapper.Map<ContratoAbonado>(contratoAbonadoDto);

            _dbContext.ContratosAbonados.Add(contratoAbonado);
            _dbContext.SaveChanges();

            var response = ApiResponse.SuccessCreated("El abonado ha sido creado.");

            return CreatedAtAction(nameof(Obtener),new {id = contratoAbonado.IdContratoAbonado},response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Editar([FromRoute] int id, [FromBody] ContratoAbonadoActualizarRequestDto contratoAbonadoDto)
        {
            var dbContratoAbonado = _dbContext.ContratosAbonados.Find(id) ?? throw new NotFoundException("Abonado no encontrado.");

            if(contratoAbonadoDto.IdVehiculo != null)
            {
                var vehiculoEncontrado = _dbContext.Vehiculos.Find(contratoAbonadoDto.IdVehiculo) ?? throw new NotFoundException("Vehículo no encontrado.");

                var tieneServicioEnMarcha = _dbContext.Servicios.Any(s => s.IdVehiculo == contratoAbonadoDto.IdVehiculo && s.EstadoPago == false);

                if(tieneServicioEnMarcha && !dbContratoAbonado.EstadoPago)
                    throw new BadRequestException("Este vehículo tiene un servicio en marcha con modalidad de hora o turno. No se pudo actualizar el abonado.");

                dbContratoAbonado.IdVehiculo = vehiculoEncontrado.IdVehiculo;
            }

            dbContratoAbonado.FechaInicio = contratoAbonadoDto.FechaInicio ?? dbContratoAbonado.FechaInicio;
            dbContratoAbonado.FechaFinal = contratoAbonadoDto.FechaFinal ?? dbContratoAbonado.FechaFinal;
            dbContratoAbonado.HoraInicio = contratoAbonadoDto.HoraInicio ?? dbContratoAbonado.HoraInicio;
            dbContratoAbonado.HoraFinal = contratoAbonadoDto.HoraFinal ?? dbContratoAbonado.HoraFinal;
            dbContratoAbonado.Monto = contratoAbonadoDto.Monto ?? dbContratoAbonado.Monto;
            dbContratoAbonado.Observacion = string.IsNullOrWhiteSpace(contratoAbonadoDto.Observacion) ? null : contratoAbonadoDto.Observacion.Trim();

            _dbContext.SaveChanges();

            var response = ApiResponse.Success("El abonado ha sido actualizado.");

            return Ok(response);
        }

        [HttpPatch("{id}/pagar")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Pagar([FromRoute] int id, [FromBody] ContratoAbonadoPagarRequestDto contratoAbonadoDto)
        {
            var dbContratoAbonado = _dbContext.ContratosAbonados.Find(id) ?? throw new NotFoundException("Abonado no encontrado.");

            if (dbContratoAbonado.EstadoPago)
                throw new BadRequestException("Este abonado ya estaba pagado.");

            var cajaChicaAbierta = _dbContext.CajasChicas.FirstOrDefault(cc => cc.Estado == true) ?? throw new NotFoundException("No hay caja chica abierta.");

            dbContratoAbonado.IdCaja = cajaChicaAbierta.IdCaja;
            dbContratoAbonado.FechaPago = contratoAbonadoDto.FechaPago;
            dbContratoAbonado.HoraPago = contratoAbonadoDto.HoraPago;
            dbContratoAbonado.TipoPago = contratoAbonadoDto.TipoPago;
            dbContratoAbonado.EstadoPago = true;

            _dbContext.SaveChanges();

            var response = ApiResponse.Success("El abonado ha sido pagado.");

            return Ok(response);
        }

        [HttpPatch("{id}/anular-pago")]
        public IActionResult AnularPago([FromRoute] int id)
        {
            var dbContratoAbonado = _dbContext.ContratosAbonados.Find(id) ?? throw new NotFoundException("Abonado no encontrado.");

            if (!dbContratoAbonado.EstadoPago)
                throw new BadRequestException("El pago de este abonado ya estaba anulado.");

            dbContratoAbonado.IdCaja = null;
            dbContratoAbonado.FechaPago = null;
            dbContratoAbonado.HoraPago = null;
            dbContratoAbonado.TipoPago = null;
            dbContratoAbonado.EstadoPago = false;

            _dbContext.SaveChanges();

            var response = ApiResponse.Success("El pago del abonado ha sido anulado.");

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            var contratoAbonado = _dbContext.ContratosAbonados.Find(id) ?? throw new NotFoundException("Abonado no encontrado.");

            _dbContext.ContratosAbonados.Remove(contratoAbonado);
            _dbContext.SaveChanges();

            var response = ApiResponse.Success("El abonado ha sido eliminado.");

            return Ok(response);
        }
    }
}
