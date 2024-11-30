using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Shared.Exceptions;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Presentation.Dtos.Common;
using ViteMontevideo_API.Presentation.Dtos.Servicios;
using ViteMontevideo_API.Presentation.Dtos.Servicios.Filtros;
using ViteMontevideo_API.Services.Parameters;
using ViteMontevideo_API.Services.Interfaces;
using ViteMontevideo_API.Helpers.Enums;

namespace ViteMontevideo_API.Services
{
    public class ServicioService : IServicioService
    {
        private readonly IServicioRepository _servicioRepository;
        private readonly IContratoAbonadoRepository _contratoAbonadoRepository;
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly ICajaChicaRepository _cajaChicaRepository;
        private readonly IMapper _mapper;

        public ServicioService(
            IServicioRepository servicioRepository, 
            IContratoAbonadoRepository contratoAbonadoRepository,
            IVehiculoRepository vehiculoRepository,
            ICajaChicaRepository cajaChicaRepository, 
            IMapper mapper)
        {
            _servicioRepository = servicioRepository;
            _contratoAbonadoRepository = contratoAbonadoRepository;
            _vehiculoRepository = vehiculoRepository;
            _cajaChicaRepository = cajaChicaRepository;
            _mapper = mapper;
        }

        public async Task<DataResponse<ServicioEntradaResponseDto>> GetAllServiciosEntrada()
        {
            var query = _servicioRepository.Query();

            query = query.Where(s => !s.EstadoPago);

            int cantidad = await query.CountAsync();

            var data = await query
                .OrderByDescending(s => s.IdServicio)
                .ProjectTo<ServicioEntradaResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new DataResponse<ServicioEntradaResponseDto>(cantidad, data);
        }

        public async Task<DataMontoResponse<ServicioSalidaResponseDto>> GetAllServiciosSalida(FiltroServicioSalida filtro)
        {
            var query = _servicioRepository.Query();

            query = query.Where(s =>
                s.EstadoPago &&
                s.FechaSalida >= filtro.FechaInicio && s.FechaSalida <= filtro.FechaFinal &&
                s.HoraSalida >= filtro.HoraInicio && s.HoraSalida <= filtro.HoraFinal
            );

            if (!string.IsNullOrWhiteSpace(filtro.Placa) && filtro.Placa.Length >= 3)
                query = query.Where(s => s.Vehiculo.Placa.Contains(filtro.Placa));

            query = filtro.Orden == Orden.ASC
                ? query.OrderBy(s => s.FechaSalida).ThenBy(s => s.HoraSalida)
                : query.OrderByDescending(s => s.FechaSalida).ThenByDescending(s => s.HoraSalida);

            int cantidad = await query.CountAsync();
            decimal totalMonto = await query.SumAsync(s => s.Monto);
            decimal totalDescuento = await query.SumAsync(s => s.Descuento);

            var data = await query
                .ProjectTo<ServicioSalidaResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new DataMontoResponse<ServicioSalidaResponseDto>(cantidad, data, totalMonto, totalDescuento);
        }

        public async Task<ServicioEntradaResponseDto> GetServicioEntradaByPlaca(string placa)
        {
            var servicio = await _servicioRepository.GetServicioEntrada(placa)
                ?? throw new NotFoundException($"El vehículo con placa {placa.ToUpper()} no se encuentra en el estacionamiento.");

            return _mapper.Map<ServicioEntradaResponseDto>(servicio);
        }

        public async Task<ServicioSalidaResponseDto> GetServicioSalidaById(int id)
        {
            var servicio = await _servicioRepository.GetServicioSalida(id)
                ?? throw new NotFoundException("Servicio con salida no encontrado.");

            return _mapper.Map<ServicioSalidaResponseDto>(servicio);
        }

        public async Task<decimal> GetServicioAmount(string placa)
        {
            var dbServicio = await _servicioRepository.GetServicioEntrada(placa) 
                ?? throw new NotFoundException($"El vehículo con placa {placa.ToUpper()} no se encuentra en el estacionamiento. No se puede calcular el monto a pagar.");

            bool esHora = dbServicio.Vehiculo.Tarifa.HoraDia == null || dbServicio.Vehiculo.Tarifa.HoraNoche == null;

            TimeZoneInfo zonaHorariaPeru = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            DateTime fechaHoraEntrada = dbServicio.FechaEntrada.Date + dbServicio.HoraEntrada;
            DateTime fechaHoraSalida = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zonaHorariaPeru);

            TimeSpan tolerancia = dbServicio.Vehiculo.Tarifa.Tolerancia;
            TimeSpan estadia = fechaHoraSalida - fechaHoraEntrada;

            decimal precioDia = dbServicio.Vehiculo.Tarifa.PrecioDia;
            decimal precioNoche = dbServicio.Vehiculo.Tarifa.PrecioNoche;

            decimal montoPagar = 0;

            if (esHora)
            {
                // Cálculo por horas
                int horasTotales = (int)estadia.TotalHours;
                montoPagar = estadia.Minutes > tolerancia.Minutes
                    ? precioDia * (horasTotales + 1)
                    : precioDia * horasTotales;
            } else
            {
                // Cálculo por turnos diurnos y nocturnos
                TimeSpan horaDia = dbServicio.Vehiculo.Tarifa.HoraDia!.Value;
                TimeSpan horaNoche = dbServicio.Vehiculo.Tarifa.HoraNoche!.Value;

                bool entradaDiurna = fechaHoraEntrada.TimeOfDay >= horaDia && fechaHoraEntrada.TimeOfDay < horaNoche;

                montoPagar += CalculateAmount(new CobroParameter(fechaHoraEntrada, fechaHoraSalida, estadia, horaDia, horaNoche, precioDia, precioNoche, tolerancia), entradaDiurna);
            }

            return montoPagar;
        }

        private static decimal CalculateAmount(CobroParameter cp, bool entradaDiurna)
        {
            decimal monto = 0;
            int dias = cp.Estadia.Days;

            bool salidaDiurna = cp.FechaSalida.TimeOfDay >= cp.HoraDia && cp.FechaSalida.TimeOfDay < cp.HoraNoche;

            if (dias == 0)
            {
                // Entrada y salida el mismo día
                TimeSpan diferenciaInicio = entradaDiurna ? cp.HoraNoche - cp.FechaEntrada.TimeOfDay : cp.HoraDia - cp.FechaEntrada.TimeOfDay;
                if (diferenciaInicio > cp.Tolerancia) monto += entradaDiurna ? cp.PrecioDia : cp.PrecioNoche;

                if (salidaDiurna)
                {
                    TimeSpan diferenciaSalida = cp.FechaSalida.TimeOfDay - cp.HoraDia;
                    if (diferenciaSalida > cp.Tolerancia) monto += cp.PrecioDia;
                }
                else
                {
                    TimeSpan diferenciaSalida = cp.FechaSalida.TimeOfDay < cp.HoraDia
                        ? (entradaDiurna ? cp.HoraNoche - cp.FechaSalida.TimeOfDay : cp.HoraDia - cp.FechaSalida.TimeOfDay)
                        : cp.FechaSalida.TimeOfDay - cp.HoraNoche;

                    if (diferenciaSalida > cp.Tolerancia) monto += entradaDiurna ? cp.PrecioNoche : cp.PrecioDia;
                }
            }
            else
            {
                // Estancia de varios días
                for (int i = 0; i < dias; i++)
                {
                    monto += cp.PrecioDia + cp.PrecioNoche;
                }

                if (salidaDiurna)
                {
                    TimeSpan diferenciaSalida = cp.FechaSalida.TimeOfDay - cp.HoraDia;
                    if (diferenciaSalida > cp.Tolerancia) monto += cp.PrecioDia;
                }
                else
                {
                    TimeSpan diferenciaSalida = cp.FechaSalida.TimeOfDay >= cp.HoraNoche
                        ? (entradaDiurna ? cp.FechaSalida.TimeOfDay - cp.HoraNoche : cp.FechaSalida.TimeOfDay - cp.HoraDia)
                        : TimeSpan.Zero;

                    if (diferenciaSalida > cp.Tolerancia) monto += cp.PrecioNoche;
                }
            }

            return monto;
        }

        public async Task<ApiResponse> Insert(ServicioCrearRequestDto servicio)
        {
            int idVehiculo = await _vehiculoRepository.GetIdVehiculoByPlaca(servicio.PlacaVehicular)
                ?? throw new BadRequestException($"El vehículo con placa {servicio.PlacaVehicular.ToUpper()} que se intentó generarle entrada no existe.");

            bool isServicioInProgress = await _servicioRepository.HasAnyServicioInProgressByIdVehiculo(idVehiculo);
            if (isServicioInProgress)
                throw new BadRequestException($"El vehículo con placa {servicio.PlacaVehicular.ToUpper()} aún está en el estacionamiento. No se le puede generar otra entrada.");

            bool isAbonoInProgress = await _contratoAbonadoRepository.HasAnyAbonoInProgressByIdVehiculo(idVehiculo);
            if (isAbonoInProgress)
                throw new BadRequestException($"El vehículo con placa {servicio.PlacaVehicular.ToUpper()} tiene un abono pendiente. No se le puede generar entrada.");

            var dbServicio = _mapper.Map<Servicio>(servicio);

            dbServicio.IdVehiculo = idVehiculo;

            dbServicio = await _servicioRepository.Insert(dbServicio);
            dbServicio = await _servicioRepository.GetServicioEntrada(dbServicio.IdServicio);
            var createdServicio = _mapper.Map<ServicioEntradaResponseDto>(dbServicio);
            return ApiResponse.Success("El servicio ha sido creado.", createdServicio);
        }

        public async Task<ApiResponse> Update(int id, ServicioActualizarRequestDto servicio)
        {
            int idVehiculo = await _vehiculoRepository.GetIdVehiculoByPlaca(servicio.PlacaVehicular)
                ?? throw new BadRequestException($"El vehículo con placa {servicio.PlacaVehicular.ToUpper()} que intentó vincular al servicio no existe.");

            var dbServicio = await _servicioRepository.GetById(id)
                ?? throw new NotFoundException("Servicio no encontrado.");

            if (dbServicio.IdVehiculo != idVehiculo)
            {
                bool isServicioInProgress = await _servicioRepository.HasAnyServicioInProgressByIdVehiculo(idVehiculo);
                if (isServicioInProgress)
                    throw new BadRequestException($"El vehículo con placa {servicio.PlacaVehicular.ToUpper()} aún está en el estacionamiento. No se le puede generar otra entrada por medio de la actualización.");

                bool isAbonoInProgress = await _contratoAbonadoRepository.HasAnyAbonoInProgressByIdVehiculo(idVehiculo);
                if (isAbonoInProgress)
                    throw new BadRequestException("El vehículo ingresado tiene un abono pendiente. No se le puede crear el servicio.");
            }

            if (dbServicio.EstadoPago && dbServicio.IdCaja.HasValue)
            {
                bool hasClosedCajaChica = await _cajaChicaRepository.IsCajaChicaClosedById(dbServicio.IdCaja.Value);
                if (hasClosedCajaChica)
                    throw new BadRequestException("La caja chica que contiene el servicio está cerrada, por lo que no se puede actualizar.");

                throw new BadRequestException("El servicio está pagado, por lo que no es posible actualizarlo.");
            }

            dbServicio.IdVehiculo = idVehiculo;
            dbServicio.Observacion = string.IsNullOrWhiteSpace(servicio.Observacion) ? null : servicio.Observacion.Trim();

            await _servicioRepository.Update(dbServicio);
            dbServicio = await _servicioRepository.GetServicioEntrada(id);
            var updatedServicio = _mapper.Map<ServicioEntradaResponseDto>(dbServicio);
            return ApiResponse.Success("El servicio ha sido actualizado.", updatedServicio);
        }

        public async Task<ApiResponse> Pay(string placa, ServicioPagarRequestDto servicio)
        {
            if (servicio.Monto < servicio.Descuento)
                throw new BadRequestException("El descuento no puede ser mayor que el monto a pagar.");

            bool existsVehiculo = await _vehiculoRepository.ExistsByPlaca(placa);
            if(!existsVehiculo)
                throw new NotFoundException($"El vehículo con placa {placa.ToUpper()} no existe.");

            var dbServicio = await _servicioRepository.GetServicioEntrada(placa)
                ?? throw new NotFoundException($"El vehículo con placa {placa.ToUpper()} no se encuentra en el estacionamiento.");

            var cajaChicaAbierta = await _cajaChicaRepository.GetOpenCajaChica()
                ?? throw new BadRequestException("No es posible actualizar el estado de pago del servicio porque no hay ninguna caja chica abierta actualmente.");

            var fechaHoraInicio = cajaChicaAbierta.FechaInicio.Date + cajaChicaAbierta.HoraInicio;
            var fechaHoraSalida = servicio.FechaSalida.Date + servicio.HoraSalida;

            if (fechaHoraInicio > fechaHoraSalida)
                throw new BadRequestException($"La fecha y hora de salida del servicio ({fechaHoraSalida:yyyy-MM-dd HH:mm:ss}) no puede ser anterior a la fecha y hora de inicio de la caja chica abierta ({fechaHoraInicio:yyyy-MM-dd HH:mm:ss}).");

            dbServicio.IdTarifa = dbServicio.Vehiculo.IdTarifa;
            dbServicio.IdCaja = cajaChicaAbierta.IdCaja;
            dbServicio.FechaSalida = servicio.FechaSalida;
            dbServicio.HoraSalida = servicio.HoraSalida;
            dbServicio.Monto = servicio.Monto;
            dbServicio.Descuento = servicio.Descuento;
            dbServicio.TipoPago = servicio.TipoPago;
            dbServicio.EstadoPago = true;

            await _servicioRepository.Update(dbServicio);
            dbServicio = await _servicioRepository.GetServicioSalida(dbServicio.IdServicio);
            var servicioPagado = _mapper.Map<ServicioSalidaResponseDto>(dbServicio);
            return ApiResponse.Success("El servicio ha sido pagado.", servicioPagado);
        }

        public async Task<ApiResponse> CancelPayment(int id)
        {
            var dbServicio = await _servicioRepository.GetServicioSalida(id) 
                ?? throw new NotFoundException($"No se ha encontrado el servicio con estado 'pagado'. No se puede anular el pago.");

            bool isCajaChicaClosed = await _cajaChicaRepository.IsCajaChicaClosedById(dbServicio.IdCaja!.Value);
            if (isCajaChicaClosed)
                throw new BadRequestException("La caja chica que contiene el servicio está cerrada. No se puede anular el pago.");

            dbServicio.IdTarifa = null;
            dbServicio.IdCaja = null;
            dbServicio.FechaSalida = null;
            dbServicio.HoraSalida = null;
            dbServicio.Monto = 0;
            dbServicio.Descuento = 0;
            dbServicio.TipoPago = null;
            dbServicio.EstadoPago = false;

            await _servicioRepository.Update(dbServicio);
            dbServicio = await _servicioRepository.GetServicioEntrada(id);
            var servicioPagoAnulado = _mapper.Map<ServicioEntradaResponseDto>(dbServicio);
            return ApiResponse.Success("El pago del servicio ha sido anulado.", servicioPagoAnulado);
        }

        public async Task<ApiResponse> DeleteById(int id)
        {
            var dbServicio = await _servicioRepository.GetById(id) 
                ?? throw new NotFoundException("Servicio no encontrado.");

            if (dbServicio.EstadoPago)
                throw new BadRequestException("El servicio está pagado, por lo que no es posible eliminarlo.");     

            await _servicioRepository.Delete(dbServicio);
            return ApiResponse.Success("El servicio ha sido eliminado.");
        }
    }
}