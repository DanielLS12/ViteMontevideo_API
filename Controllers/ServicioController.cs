#pragma warning disable CS8602
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.ActionFilters;
using ViteMontevideo_API.Dtos.Common;
using ViteMontevideo_API.Dtos.Servicios;
using ViteMontevideo_API.Dtos.Servicios.Filtros;
using ViteMontevideo_API.Middleware.Exceptions;
using ViteMontevideo_API.models;

namespace ViteMontevideo_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ServicioController : ControllerBase
    {
        private readonly EstacionamientoContext _dbContext;
        private readonly IMapper _mapper;

        public ServicioController(EstacionamientoContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Listar([FromQuery] FiltroServicio fs)
        {
            var query = _dbContext.Servicios.Include(v => v.OVehiculo).AsQueryable();

            query = query.Where(s => s.EstadoPago == fs.EstadoPago);
                
            query = query.Where(s => fs.EstadoPago ? s.FechaSalida >= fs.FechaInicio : s.FechaEntrada >= fs.FechaInicio);
                
            query = query.Where(s => fs.EstadoPago ? s.FechaSalida <= fs.FechaFinal : s.FechaEntrada <= fs.FechaFinal);

            query = query.Where(s => fs.EstadoPago ? s.HoraSalida >= fs.HoraInicio : s.HoraEntrada >= fs.HoraInicio);
                
            query = query.Where(s => fs.EstadoPago ? s.HoraSalida <= fs.HoraFinal : s.HoraEntrada <= fs.HoraFinal);


            if (!string.IsNullOrWhiteSpace(fs.Placa) && fs.Placa.Length >= 3)
            {
                query = query.Where(s => s.OVehiculo.Placa.Contains(fs.Placa));
            }

            query = fs.Orden == "asc"
                ? query.OrderBy(s => s.FechaEntrada).ThenBy(s => s.HoraEntrada)
                : query.OrderByDescending(s => s.FechaEntrada).ThenByDescending(s => s.HoraEntrada);

            var servicios = query
                .AsNoTracking()
                .OrderByDescending(s => s.IdServicio)
                .ProjectTo<ServicioResponseDto>(_mapper.ConfigurationProvider)
                .ToList();

            int totalRegistros = servicios.Count;
            decimal totalMonto = servicios.Sum(s => s.Monto);
            decimal totalDescuento = servicios.Sum(s => s.Descuento);

            var resultado = new
            {
                TotalRegistros = totalRegistros,
                TotalMonto = totalMonto,
                TotalDescuento = totalDescuento,
                Registros = servicios
            };

            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public IActionResult Obtener(int id)
        {
            var servicio = _dbContext.Servicios
                .AsNoTracking()
                .ProjectTo<ServicioResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefault(s => s.IdServicio == id) ?? throw new NotFoundException("Servicio no encontrado.");       

            return Ok(servicio);
        }

        [HttpGet("{placa}/entrada-vehicular")]
        public IActionResult ObtenerEntrada(string placa)
        {
            var servicio = _dbContext.Servicios
                .AsNoTracking()
                .Include(v => v.OVehiculo)
                .Select(s => new
                {
                    s.IdServicio,
                    s.IdVehiculo,
                    s.OVehiculo.Placa,
                    Modalidad = s.IdTarifa == null ? (s.OVehiculo.OTarifa.HoraDia == null ? "Hora" : "Turno") : (s.OTarifa.HoraDia == null ? "Hora" : "Turno"),
                    s.FechaEntrada,
                    s.HoraEntrada,
                    s.Observacion,
                    s.EstadoPago
                })
                .FirstOrDefault(s => s.Placa == placa.ToUpper() && s.EstadoPago == false) ??
                throw new NotFoundException("Este vehiculo no se encuentra en el estacionamiento en este momento.");     

            return Ok(servicio);
        }

        [HttpGet("{id}/generar-monto")]
        public IActionResult GenerarMonto(int id)
        {
            var oServicio = _dbContext.Servicios
                    .Include(v => v.OVehiculo)
                        .ThenInclude(t => t.OTarifa)
                    .FirstOrDefault(s => s.IdServicio == id) ?? throw new NotFoundException("Servicio no encontrado.");

            bool EsHora = oServicio.OVehiculo.OTarifa.HoraDia == null;

            TimeZoneInfo zonaHorariaPeru = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");

            DateTime fechaHoraEntrada = oServicio.FechaEntrada + oServicio.HoraEntrada, fechaHoraSalida = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zonaHorariaPeru);

            TimeSpan tolerancia = oServicio.OVehiculo.OTarifa.Tolerancia, estadia = fechaHoraSalida - fechaHoraEntrada;

            decimal precioDia = oServicio.OVehiculo.OTarifa.PrecioDia, precioNoche = oServicio.OVehiculo.OTarifa.PrecioNoche;

            if (EsHora)
            {
                int horasTotales = (int)estadia.TotalHours;

                oServicio.Monto = tolerancia.Minutes > estadia.Minutes ?
                                    precioDia * horasTotales : precioDia * (horasTotales + 1);
            }
            else
            {
                TimeSpan hora_dia = (TimeSpan)oServicio.OVehiculo.OTarifa.HoraDia,
                            hora_noche = (TimeSpan)oServicio.OVehiculo.OTarifa.HoraNoche;

                int dias = estadia.Days;

                decimal monto = 0;

                //Entrada diurna
                if (hora_dia <= fechaHoraEntrada.TimeOfDay && hora_noche > fechaHoraEntrada.TimeOfDay)
                {
                    //Todo correcto
                    if (estadia.Days == 0)
                    {
                        TimeSpan diferenciaTiempoComienzo = hora_noche - fechaHoraEntrada.TimeOfDay;
                        monto += diferenciaTiempoComienzo > tolerancia ? precioDia : 0;

                        //Salida Diurna - Correcto
                        if (hora_dia <= fechaHoraSalida.TimeOfDay && hora_noche > fechaHoraSalida.TimeOfDay)
                        {
                            if (fechaHoraEntrada.Date != fechaHoraSalida.Date)
                            {
                                monto += precioNoche;

                                TimeSpan diferenciaTiempoSalida2 = fechaHoraSalida.TimeOfDay - hora_dia;
                                monto += diferenciaTiempoSalida2 > tolerancia ? precioDia : 0;
                            }
                        }
                        else //Salida nocturna - Correcto
                        {
                            TimeSpan diferenciaTiempoSalida = hora_dia > fechaHoraSalida.TimeOfDay ?
                                                            hora_noche - fechaHoraSalida.TimeOfDay : fechaHoraSalida.TimeOfDay - hora_noche;
                            monto += diferenciaTiempoSalida > tolerancia ? precioNoche : 0;
                        }
                    }
                    else //Todo correcto
                    {
                        for (int i = 0; i <= estadia.Days; i++)
                        {
                            if (i < estadia.Days)
                            {
                                monto += precioDia;
                                monto += precioNoche;
                                continue;
                            }

                            //Salida diurna
                            if (hora_dia <= fechaHoraSalida.TimeOfDay && hora_noche > fechaHoraSalida.TimeOfDay)
                            {
                                TimeSpan diferenciaTiempoSalida = fechaHoraSalida.TimeOfDay - hora_dia;
                                monto += diferenciaTiempoSalida > tolerancia ? precioDia : 0;
                            }
                            else //Salida nocturna
                            {
                                TimeSpan diferenciaTiempoSalida = hora_dia > fechaHoraSalida.TimeOfDay ?
                                                        hora_noche - fechaHoraSalida.TimeOfDay : fechaHoraSalida.TimeOfDay - hora_noche;
                                monto += diferenciaTiempoSalida > tolerancia ? precioNoche : 0;
                                monto += precioDia;
                            }
                        }
                    }
                }
                else //Entrada Nocturna
                {
                    //Todo correcto
                    if (estadia.Days == 0)
                    {
                        TimeSpan diferenciaTiempoComienzo = hora_dia > fechaHoraEntrada.TimeOfDay ?
                                        hora_dia - fechaHoraEntrada.TimeOfDay : TimeSpan.MaxValue;
                        monto += diferenciaTiempoComienzo > tolerancia ? precioNoche : 0;

                        //Salida diurna
                        if (hora_dia <= fechaHoraSalida.TimeOfDay && hora_noche > fechaHoraSalida.TimeOfDay)
                        {
                            TimeSpan diferenciaTiempoSalida = fechaHoraSalida.TimeOfDay - hora_dia;
                            monto += diferenciaTiempoSalida > tolerancia ? precioDia : 0;
                        }
                        else //Salida Nocturna
                        {
                            if(monto == 0)
                            {
                                TimeSpan diferenciaTiempoSalida = hora_dia > fechaHoraSalida.TimeOfDay ?
                                    hora_dia - fechaHoraSalida.TimeOfDay : fechaHoraSalida.TimeOfDay - hora_noche;
                                monto += diferenciaTiempoSalida > tolerancia ? precioNoche : 0;
                                monto += precioDia;
                            }
                        }            
                    }
                    else
                    {
                        //Todo correcto
                        for (int i = 0; i <= estadia.Days; i++)
                        {
                            if (i < estadia.Days)
                            {
                                monto += precioNoche;
                                monto += precioDia;
                                continue;
                            }

                            //Salida diurna
                            if (hora_dia <= fechaHoraSalida.TimeOfDay && hora_noche > fechaHoraSalida.TimeOfDay)
                            {
                                TimeSpan diferenciaTiempoSalida = fechaHoraSalida.TimeOfDay - hora_dia;
                                monto += diferenciaTiempoSalida > tolerancia ? precioDia : 0;
                                monto += precioNoche;
                            }
                            else //Salida Nocturna
                            {
                                TimeSpan diferenciaTiempoSalida = hora_dia > fechaHoraSalida.TimeOfDay ?
                                    TimeSpan.MaxValue : fechaHoraSalida.TimeOfDay - hora_noche;
                                monto += diferenciaTiempoSalida > tolerancia ? precioNoche : 0;
                            }
                        }
                    }
                }
                oServicio.Monto = monto;
            }

            return Ok(oServicio.Monto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Guardar(ServicioRequestDto servicioDto)
        {
            var vehiculo = _dbContext.Vehiculos.Find(servicioDto.IdVehiculo) ?? throw new NotFoundException("Vehículo no encontrado.");

            bool vehiculoConEntrada = _dbContext.Servicios.Any(s => s.EstadoPago == false && s.IdVehiculo == servicioDto.IdVehiculo);

            if (vehiculoConEntrada) 
                throw new BadRequestException("El vehículo aún está en el estacionamiento. No se puede crear otra entrada.");

            var servicio = _mapper.Map<Servicio>(servicioDto);
            _dbContext.Servicios.Add(servicio);
            _dbContext.SaveChanges();

            var response = ApiResponse.SuccessCreated("El servicio ha sido creado.");

            return CreatedAtAction(nameof(Obtener),new {id = servicio.IdServicio},response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Editar([FromRoute] int id,[FromBody] ServicioRequestDto servicio) 
        {
            if (servicio.Descuento > servicio.Monto || servicio.Descuento < 0)
                throw new BadRequestException("El descuento no puede ser mayor al monto o valor negativo.");

            var dbServicio = _dbContext.Servicios.Find(id) ?? throw new NotFoundException("Servicio no encontrado.");

            var dbVehiculo = _dbContext.Vehiculos.Find(servicio.IdVehiculo) ?? throw new NotFoundException("Vehículo no encontrado.");

            dbServicio.IdVehiculo = dbVehiculo.IdVehiculo;
            dbServicio.IdTarifa = servicio.EstadoPago ? dbVehiculo.IdTarifa : null;
            dbServicio.IdCaja = null;
            dbServicio.FechaEntrada = servicio.FechaEntrada;
            dbServicio.HoraEntrada = servicio.HoraEntrada;
            dbServicio.FechaSalida = null;
            dbServicio.HoraSalida = null;
            dbServicio.TipoPago = null;
            dbServicio.Monto = servicio.Monto;
            dbServicio.Descuento = servicio.Descuento;
            dbServicio.Observacion = string.IsNullOrWhiteSpace(servicio.Observacion) ? null : servicio.Observacion.Trim();
            dbServicio.EstadoPago = servicio.EstadoPago;

            if (servicio.EstadoPago)
            {
                if (servicio.FechaSalida == null || servicio.HoraSalida == null || servicio.TipoPago == null)
                    throw new BadRequestException("La fecha y hora de salida, y el tipo de pago no pueden estar vacios.");

                dbServicio.FechaSalida = servicio.FechaSalida;
                dbServicio.HoraSalida = servicio.HoraSalida;
                dbServicio.TipoPago = servicio.TipoPago;

                var cajaChicaAbierta = _dbContext.CajasChicas.FirstOrDefault(cc => cc.Estado == true) ?? throw new NotFoundException("No hay caja chica abierta.");

                dbServicio.IdCaja = cajaChicaAbierta.IdCaja;
            }

            _dbContext.SaveChanges();

            var response = ApiResponse.Success("El servicio ha sido actualizado.");

            return Ok(response);
        }

        [HttpPatch("{placa}/pagar")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Pagar([FromRoute] string placa, [FromBody] ServicioRequestDto servicio)
        {
            if (string.IsNullOrWhiteSpace(placa) || placa.Length != 6)
                throw new BadRequestException("La placa no puede estar vacia y tiene que tener 6 caracteres.");

            if (servicio.Descuento > servicio.Monto || servicio.Descuento < 0)
                throw new BadRequestException("El descuento no puede ser mayor al monto o valor negativo.");

            if (servicio.FechaSalida == null || servicio.HoraSalida == null || servicio.TipoPago == null)
                throw new BadRequestException("La fecha y hora de salida, y el tipo de pago no pueden estar vacios.");

            var cajaChicaAbierta = _dbContext.CajasChicas.FirstOrDefault(cc => cc.Estado == true) ?? throw new NotFoundException("No hay caja chica abierta.");

            var dbServicio = _dbContext.Servicios
                                .Include(s => s.OVehiculo)
                                .FirstOrDefault(s => s.OVehiculo.Placa == placa.Trim() && s.EstadoPago == false) ?? throw new NotFoundException("Este vehículo no se encuentra en el estacionamiento.");

            if (dbServicio.EstadoPago)
                throw new BadRequestException("Este servicio ya ha sido pagado.");

            dbServicio.IdTarifa = dbServicio.OVehiculo.IdTarifa;
            dbServicio.IdCaja = cajaChicaAbierta.IdCaja;
            dbServicio.FechaSalida = servicio.FechaSalida;
            dbServicio.HoraSalida = servicio.HoraSalida;
            dbServicio.Monto = servicio.Monto;
            dbServicio.Descuento = servicio.Descuento;
            dbServicio.TipoPago = servicio.TipoPago;
            dbServicio.EstadoPago = true;

            _dbContext.SaveChanges();

            var response = ApiResponse.Success("El servicio ha sido pagado.");

            return Ok(response);
        }

        [HttpPatch("{id}/anular-pago")]
        public IActionResult AnularPago(int id)
        {
            var dbServicio = _dbContext.Servicios.Find(id) ?? throw new NotFoundException("Servicio no encontrado.");

            dbServicio.IdTarifa = null;
            dbServicio.IdCaja = null;
            dbServicio.FechaSalida = null;
            dbServicio.HoraSalida = null;
            dbServicio.Monto = 0;
            dbServicio.Descuento = 0;
            dbServicio.TipoPago = null;
            dbServicio.EstadoPago = false;

            _dbContext.SaveChanges();

            var response = ApiResponse.Success("El pago del servicio ha sido anulado.");

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            var servicio = _dbContext.Servicios.Find(id) ?? throw new NotFoundException("Servicio no encontrado.");     

            _dbContext.Servicios.Remove(servicio);
            _dbContext.SaveChanges();

            var response = ApiResponse.Success("El servicio ha sido eliminado.");

            return Ok(response);
        }
    }
}
