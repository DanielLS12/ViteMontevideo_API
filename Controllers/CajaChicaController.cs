using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.ActionFilters;
using ViteMontevideo_API.Dtos.CajasChicas;
using ViteMontevideo_API.Dtos.CajasChicas.Filtros;
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
        public IActionResult Listar([FromQuery] FiltroCajaChica filtro)
        {
            var query = _dbContext.CajasChicas.AsQueryable();

            query = query.Where(c => c.FechaInicio >= filtro.FechaInicio && c.FechaInicio <= filtro.FechaFinal);

            if (!string.IsNullOrWhiteSpace(filtro.Turno))
            {
                query = query.Where(c => c.Turno.Contains(filtro.Turno));
            }

            var data = query
                .AsNoTracking()
                .OrderByDescending(caja => caja.IdCaja)
                .ProjectTo<CajaChicaResponseDto>(_mapper.ConfigurationProvider)
                .ToList();

            int cantidad = data.Count;

            return Ok(new DataResponse<CajaChicaResponseDto>(cantidad, data));
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
                        .Include(c => c.Trabajador)
                        .Include(c => c.Servicios)
                            .ThenInclude(t => t.Tarifa)
                                .ThenInclude(a => a.Actividad)
                        .Include(c => c.Egresos)
                        .Include(c => c.ContratosAbonados)
                            .ThenInclude(c => c.Vehiculo)
                        .Include(c => c.ComerciosAdicionales)
                            .ThenInclude(c => c.Cliente)
                        .Where(c => c.FechaInicio == fecha)
                        .Select(cajaChica => new InformeCajaChica
                        {
                            Cajero = $"{cajaChica.Trabajador.Nombre} {cajaChica.Trabajador.ApellidoPaterno} {cajaChica.Trabajador.ApellidoMaterno}",
                            MParticulares = cajaChica.Servicios.Where(s => s.Tarifa.Actividad.Nombre == "Particular").Sum(s => s.Monto),
                            MTurnos = cajaChica.Servicios.Where(s => s.Tarifa.HoraDia != null && s.Tarifa.Actividad.Nombre != "EsSalud").Sum(s => s.Monto),
                            MEsSalud = cajaChica.Servicios.Where(s => s.Tarifa.Actividad.Nombre == "EsSalud").Sum(s => s.Monto),
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
        public IActionResult Guardar(CajaChicaCrearRequestDto cajaChicaDto)
        {
            var trabajador = _dbContext.Trabajadores.Find(cajaChicaDto.IdTrabajador) ?? throw new NotFoundException("Trabajador no encontrado.");

            if (!trabajador.Estado)
                throw new BadRequestException("Trabajador no activo.");       

            var cajaChicaAbierta = _dbContext.CajasChicas.Any(c => c.Estado == true);

            if (cajaChicaAbierta)
                throw new BadRequestException("Ya existe una caja chica abierta.");    

            var cajaChica = _mapper.Map<CajaChica>(cajaChicaDto);

            cajaChica.Estado = true;

            _dbContext.CajasChicas.Add(cajaChica);
            _dbContext.SaveChanges();

            var response = ApiResponse.SuccessCreated("La caja chica ha sido creada.");

            return CreatedAtAction(nameof(Obtener),new {id = cajaChica.IdCaja },response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Editar([FromRoute] int id,[FromBody] CajaChicaActualizarRequestDto cajaChicaDto)
        {
            var dbCajaChica = _dbContext.CajasChicas.Find(id) ?? throw new NotFoundException("Caja chica no encontrada.");

            if(cajaChicaDto.IdTrabajador != null)
            {
                var trabajador = _dbContext.Trabajadores.Find(cajaChicaDto.IdTrabajador) ?? throw new NotFoundException("Trabajador no encontrado.");

                if (!trabajador.Estado)
                    throw new BadRequestException("Trabajador no activo.");
            }

            dbCajaChica.IdTrabajador = cajaChicaDto.IdTrabajador ?? dbCajaChica.IdTrabajador;
            dbCajaChica.Observacion = string.IsNullOrWhiteSpace(cajaChicaDto.Observacion) ? null : cajaChicaDto.Observacion.Trim();
            dbCajaChica.SaldoInicial = cajaChicaDto.SaldoInicial ?? dbCajaChica.SaldoInicial;
            dbCajaChica.Turno = cajaChicaDto.Turno ?? dbCajaChica.Turno;

            _dbContext.SaveChanges();

            var response = ApiResponse.Success("La caja chica ha sido actualizada.");

            return Ok(response);
        }

        [HttpPatch("{id}/abrir")]
        public IActionResult Abrir(int id)
        {
            var dbCajaChica = _dbContext.CajasChicas.Find(id) ?? throw new NotFoundException("Caja chica no encontrada.");

            if (dbCajaChica.Estado)
                throw new BadRequestException("Esta caja chica ya estaba abierta.");

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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Cerrar([FromRoute] int id, [FromBody] CajaChicaCerrarRequestDto cajaChicaDto)
        {
            var dbCajaChica = _dbContext.CajasChicas.Find(id) ?? throw new NotFoundException("Caja chica no encontrada.");

            if (!dbCajaChica.Estado)
                throw new BadRequestException("Esta caja chica ya se encontraba cerrada.");

            dbCajaChica.FechaFinal = cajaChicaDto.FechaFinal;
            dbCajaChica.HoraFinal = cajaChicaDto.HoraFinal;
            dbCajaChica.Observacion = string.IsNullOrWhiteSpace(cajaChicaDto.Observacion) ? null : cajaChicaDto.Observacion.Trim();
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
                throw new BadRequestException("No se puede eliminar esta caja porque contiene egresos, abonados o servicios.");         

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
