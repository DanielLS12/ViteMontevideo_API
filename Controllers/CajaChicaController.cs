using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.ActionFilters;
using ViteMontevideo_API.Dtos.CajasChicas;
using ViteMontevideo_API.Dtos.Common;
using ViteMontevideo_API.Middleware.Exceptions;
using ViteMontevideo_API.models;
using ViteMontevideo_API.Models;

namespace ViteMontevideo_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CajaChicaController : ControllerBase
    {
        private readonly EstacionamientoContext _dbContext;
        private readonly IMapper _mapper;

        public CajaChicaController(EstacionamientoContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Listar(DateTime? fechaInicio, DateTime? fechaFinal, string? turno)
        {
            var query = _dbContext.CajasChicas.AsQueryable();

            // Aplicar los filtros si están presentes
            if (fechaInicio.HasValue && fechaFinal.HasValue)
            {
                query = query.Where(c => c.FechaInicio >= fechaInicio.Value && c.FechaInicio <= fechaFinal.Value);
            }
            else if (fechaInicio.HasValue)
            {
                query = query.Where(c => c.FechaInicio >= fechaInicio.Value);
            }
            else if (fechaFinal.HasValue)
            {
                query = query.Where(c => c.FechaInicio <= fechaFinal.Value);
            }

            if (!string.IsNullOrEmpty(turno))
            {
                query = query.Where(c => c.Turno.Contains(turno));
            }

            var cajasChicas = query
                                .AsNoTracking()
                                .OrderByDescending(caja => caja.IdCaja)
                                .ProjectTo<CajaChicaResponseDto>(_mapper.ConfigurationProvider)
                                .ToList();

            return Ok(cajasChicas);
        }

        [HttpGet("{id}")]
        public IActionResult Obtener(int id)
        {
            var cajaChica = _dbContext.CajasChicas
                                .AsNoTracking()
                                .OrderByDescending(caja => caja.IdCaja)
                                .ProjectTo<CajaChicaResponseDto>(_mapper.ConfigurationProvider)
                                .SingleOrDefault(c => c.IdCaja == id) ?? throw new NotFoundException("Caja Chica no encontrada.");

            return Ok(cajaChica);
        }

        [HttpGet("{fecha}/informe")]
        public IActionResult ObtenerInformes(DateTime fecha)
        {
            var informes = _dbContext.CajasChicas
                        .AsNoTracking()
                        .Include(c => c.OTrabajador)
                        .Include(c => c.Servicios)
                            .ThenInclude(t => t.OTarifa)
                                .ThenInclude(a => a.OActividad)
                        .Include(c => c.Egresos)
                        .Include(c => c.ContratosAbonados)
                            .ThenInclude(c => c.OVehiculo)
                        .Include(c => c.ComerciosAdicionales)
                            .ThenInclude(c => c.OCliente)
                        .Where(c => c.FechaInicio == fecha)
                        .Select(cajaChica => new InformeCajaChica
                        {
                            Cajero = $"{cajaChica.OTrabajador.Nombre} {cajaChica.OTrabajador.ApellidoPaterno} {cajaChica.OTrabajador.ApellidoMaterno}",
                            MParticulares = cajaChica.Servicios.Where(s => s.OTarifa.OActividad.Nombre == "Particular").Sum(s => s.Monto),
                            MTurnos = cajaChica.Servicios.Where(s => s.OTarifa.HoraDia != null && s.OTarifa.OActividad.Nombre != "EsSalud").Sum(s => s.Monto),
                            MEsSalud = cajaChica.Servicios.Where(s => s.OTarifa.OActividad.Nombre == "EsSalud").Sum(s => s.Monto),
                            MEfectivo = SumarPorTipoPagoServicio(cajaChica.Servicios, "Efectivo") + 
                                        SumarPorTipoPagoAbonados(cajaChica.ContratosAbonados, "Efectivo") +
                                        SumarPorTipoPagoComerciosAdicionales(cajaChica.ComerciosAdicionales, "Efectivo") -
                                        cajaChica.Egresos.Sum(e => e.Monto),
                            MYape = SumarPorTipoPagoServicio(cajaChica.Servicios, "Yape") +
                                    SumarPorTipoPagoAbonados(cajaChica.ContratosAbonados, "Yape") +
                                    SumarPorTipoPagoComerciosAdicionales(cajaChica.ComerciosAdicionales, "Yape"),
                            MOtros = SumarPorTipoPagoServicio(cajaChica.Servicios, "Otros") +
                                        SumarPorTipoPagoAbonados(cajaChica.ContratosAbonados, "Otros") +
                                        SumarPorTipoPagoComerciosAdicionales(cajaChica.ComerciosAdicionales, "Otros"),
                            MServicios = cajaChica.Servicios.Sum(s => s.Monto),
                            MContratosAbonados = cajaChica.ContratosAbonados.Sum(ca => ca.Monto),
                            MComerciosAdicionales = cajaChica.ComerciosAdicionales.Sum(cad => cad.Monto),
                            MEgresos = cajaChica.Egresos.Sum(e => e.Monto),
                            ComerciosAdicionales = cajaChica.ComerciosAdicionales,
                            Egresos = cajaChica.Egresos,
                            Abonados = cajaChica.ContratosAbonados
                        })
                        .ToList();

            return Ok(informes);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Guardar(CajaChicaRequestDto cajaChicaDto)
        {
            var trabajador = _dbContext.Trabajadores.Find(cajaChicaDto.IdTrabajador) ?? throw new NotFoundException("Trabajador no encontrado.");

            if (!trabajador.Estado)
                throw new BadRequestException("Trabajador no activo.");
            

            var cajaChicaAbierta = _dbContext.CajasChicas.Any(c => c.Estado == true);

            if (cajaChicaAbierta)
                throw new BadRequestException("Ya existe una caja chica abierta.");    

            var cajaChica = _mapper.Map<CajaChica>(cajaChicaDto);

            _dbContext.CajasChicas.Add(cajaChica);
            _dbContext.SaveChanges();

            var response = ApiResponse.SuccessCreated("La caja chica ha sido creada.");

            return CreatedAtAction(nameof(Obtener),new {id = cajaChica.IdCaja },response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Editar([FromRoute] int id,[FromBody] CajaChicaRequestDto cajaChicaDto)
        {
            var dbCajaChica = _dbContext.CajasChicas.Find(id) ?? throw new NotFoundException("Caja chica no encontrada.");

            var trabajador = _dbContext.Trabajadores.Find(cajaChicaDto.IdTrabajador) ?? throw new NotFoundException("Trabajador no encontrado.");

            if (!trabajador.Estado)
                throw new BadRequestException("Trabajador no activo.");

            dbCajaChica.IdTrabajador = cajaChicaDto.IdTrabajador;
            dbCajaChica.FechaInicio = cajaChicaDto.FechaInicio;
            dbCajaChica.HoraInicio = cajaChicaDto.HoraInicio;
            dbCajaChica.Observacion = string.IsNullOrWhiteSpace(cajaChicaDto.Observacion) ? null : cajaChicaDto.Observacion.Trim();
            dbCajaChica.SaldoInicial = cajaChicaDto.SaldoInicial;
            dbCajaChica.Turno = cajaChicaDto.Turno;

            _dbContext.SaveChanges();

            var response = ApiResponse.Success("La caja chica ha sido actualizada.");

            return Ok(response);
        }

        [HttpPatch("{id}/abrir")]
        public IActionResult Abrir(int id)
        {
            var dbCajaChica = _dbContext.CajasChicas.Find(id) ?? throw new NotFoundException("Caja chica no encontrada.");

            if (dbCajaChica.Estado)
                throw new BadRequestException("Esta caja chica ya esta abierta.");

            var cajaChicaAbierta = _dbContext.CajasChicas.Any(cc => cc.Estado == true);

            if (cajaChicaAbierta)
                throw new BadRequestException("Ya existe un caja chica abierta.");

            dbCajaChica.FechaFinal = null;
            dbCajaChica.HoraFinal = null;
            dbCajaChica.Estado = true;

            _dbContext.SaveChanges();

            var response = ApiResponse.Success("La caja chica ha sido abierta.");

            return Ok(response);
        }

        [HttpPatch("{id}/cerrar")]
        public IActionResult Cerrar([FromRoute] int id, [FromBody] CajaChicaCerrarRequestDto cajaChicaDto)
        {
            if (cajaChicaDto.FechaFinal == null || cajaChicaDto.HoraFinal == null)
                throw new BadRequestException("La fecha y hora final no pueden estar vacias.");

            var dbCajaChica = _dbContext.CajasChicas.Find(id) ?? throw new NotFoundException("Caja chica no encontrada.");

            if (!dbCajaChica.Estado)
                throw new BadRequestException("Esta caja chica ya se encuentra cerrada.");

            dbCajaChica.FechaFinal = cajaChicaDto.FechaFinal;
            dbCajaChica.HoraFinal = cajaChicaDto.HoraFinal;
            dbCajaChica.Estado = false;

            _dbContext.SaveChanges();

            var response = ApiResponse.Success("La caja chica ha sido cerrada.");

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            var cajaChica = _dbContext.CajasChicas.Find(id) ?? throw new NotFoundException("Caja chica no encontrada.");

            var tieneContratosAbonados = _dbContext.ContratosAbonados.Any(ca => ca.IdCaja == id);

            var tieneEgresos = _dbContext.Egresos.Any(ca => ca.IdCaja == id);

            var tieneServicios = _dbContext.Servicios.Any(ca => ca.IdCaja == id);

            if (tieneContratosAbonados || tieneEgresos || tieneServicios)
                throw new BadRequestException("No se puede eliminar esta caja porque existen egresos, abonados o servicios vinculadas a esta.");         

            _dbContext.CajasChicas.Remove(cajaChica);
            _dbContext.SaveChanges();

            var response = ApiResponse.Success("La caja chica ha sido eliminada.");

            return Ok(response);
        }

        private static decimal SumarPorTipoPagoServicio(IEnumerable<Servicio> servicios, string tipoPago)
        {
            decimal montoTotal = servicios.Where(s => s.TipoPago == tipoPago).Sum(s => s.Monto);
            return montoTotal;
        }

        private static decimal SumarPorTipoPagoAbonados(IEnumerable<ContratoAbonado> abonados, string tipoPago)
        {
            decimal montoTotal = abonados.Where(s => s.TipoPago == tipoPago).Sum(s => s.Monto);
            return montoTotal;
        }

        private static decimal SumarPorTipoPagoComerciosAdicionales(IEnumerable<ComercioAdicional> ca, string tipoPago)
        {
            decimal montoTotal = ca.Where(s => s.TipoPago == tipoPago).Sum(s => s.Monto);
            return montoTotal;
        }
    }
}
