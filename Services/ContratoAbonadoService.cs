﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Exceptions;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Presentation.Dtos.Common;
using ViteMontevideo_API.Presentation.Dtos.ContratosAbonado;
using ViteMontevideo_API.Presentation.Dtos.ContratosAbonado.Filtros;
using ViteMontevideo_API.Services.Interfaces;

namespace ViteMontevideo_API.Services
{
    public class ContratoAbonadoService : IContratoAbonadoService
    {
        private readonly IContratoAbonadoRepository _contratoAbonadoRepository;
        private readonly IServicioRepository _servicioRepository;
        private readonly ICajaChicaRepository _cajaChicaRepository;
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly IMapper _mapper;

        public ContratoAbonadoService(
            IContratoAbonadoRepository contratoAbonadoRepository, 
            IServicioRepository servicioRepository,
            ICajaChicaRepository cajaChicaRepository,
            IVehiculoRepository vehiculoRepository, 
            IMapper mapper)
        {
            _contratoAbonadoRepository = contratoAbonadoRepository;
            _servicioRepository = servicioRepository;
            _cajaChicaRepository = cajaChicaRepository;
            _vehiculoRepository = vehiculoRepository;
            _mapper = mapper;
        }

        public async Task<PageCursorMontoResponse<ContratoAbonadoResponseDto>> GetAllPageCursor(FiltroContratoAbonado filtro)
        {
            var query = _contratoAbonadoRepository.Query();

            if (!string.IsNullOrWhiteSpace(filtro.Placa) && filtro.Placa.Length >= 3)
                query = query.Where(ca => ca.Vehiculo.Placa.Contains(filtro.Placa));

            if (filtro.EstadoPago.HasValue)
                query = query.Where(ca => ca.EstadoPago == filtro.EstadoPago);

            int cantidad = await query.CountAsync();
            decimal totalMonto = await query.SumAsync(ca => ca.Monto);

            query = _contratoAbonadoRepository.ApplyPageCursor(query, filtro.Cursor, filtro.Count, ca => ca.IdContratoAbonado);

            var data = await query
                .OrderByDescending(ca => ca.IdContratoAbonado)
                .ProjectTo<ContratoAbonadoResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            int siguienteCursor = data.Any() ? (data.LastOrDefault()?.IdContratoAbonado ?? 0) : 0;

            if (siguienteCursor == 0)
                cantidad = 0;

            return new PageCursorMontoResponse<ContratoAbonadoResponseDto>(cantidad, siguienteCursor, data, totalMonto);
        }

        public async Task<ContratoAbonadoResponseDto> GetById(int id)
        {
            var contratoAbonado = await _contratoAbonadoRepository.GetById(id);
            return _mapper.Map<ContratoAbonadoResponseDto>(contratoAbonado);
        }

        public async Task<ApiResponse> Insert(ContratoAbonadoCrearRequestDto abonado)
        {
            if (abonado.FechaInicio > abonado.FechaFinal)
                throw new BadRequestException("La fecha de inicio no puede ser mayor que la fecha final");

            bool existsVehiculo = await _vehiculoRepository.ExistsById(abonado.IdVehiculo);
            if (!existsVehiculo)
                throw new BadRequestException("El vehículo que intento vincular al abono no existe.");

            bool hasAnyServicioInProgress = await _servicioRepository.HasAnyInProgressByIdVehiculo(abonado.IdVehiculo);
            if (hasAnyServicioInProgress)
                throw new BadRequestException("El vehículo ingresado tiene un servicio (hora o turno) en marcha. No se le puede crear el abono.");

            bool hasAnyAbonoInProgress = await _contratoAbonadoRepository.HasAnyInProgressByIdVehiculo(abonado.IdVehiculo);
            if (hasAnyAbonoInProgress)
                throw new BadRequestException("El vehículo ingresado tiene un abono pendiente. No se le puede crear otro abono.");

            var dbContratoAbonado = _mapper.Map<ContratoAbonado>(abonado);
            dbContratoAbonado = await _contratoAbonadoRepository.Insert(dbContratoAbonado);
            dbContratoAbonado = await _contratoAbonadoRepository.GetById(dbContratoAbonado.IdContratoAbonado);
            var createdContratoAbonado = _mapper.Map<ContratoAbonadoResponseDto>(dbContratoAbonado);
            return ApiResponse.Success("El abono ha sido creado.", createdContratoAbonado);
        }

        public async Task<ApiResponse> Update(int id, ContratoAbonadoActualizarRequestDto abonado)
        {
            if (abonado.FechaInicio > abonado.FechaFinal)
                throw new BadRequestException("La fecha de inicio no puede ser mayor que la fecha final");

            var dbContratoAbonado = await _contratoAbonadoRepository.GetById(id);

            if(dbContratoAbonado.EstadoPago)
            {
                bool hasClosedCajaChica = await _contratoAbonadoRepository.HasClosedCajaChicaById(id);
                if (hasClosedCajaChica)
                    throw new BadRequestException("La caja chica en donde el abono se encuentra está cerrada. Por lo tanto, no se puede actualizar.");

                throw new BadRequestException("El abono está pagado, por lo que no es posible actualizarlo.");
            }

            dbContratoAbonado.FechaInicio = abonado.FechaInicio;
            dbContratoAbonado.FechaFinal = abonado.FechaFinal;
            dbContratoAbonado.HoraInicio = abonado.HoraInicio;
            dbContratoAbonado.HoraFinal = abonado.HoraFinal;
            dbContratoAbonado.Monto = abonado.Monto;
            dbContratoAbonado.Observacion = string.IsNullOrWhiteSpace(abonado.Observacion) ? null : abonado.Observacion.Trim();
            
            await _contratoAbonadoRepository.Update(dbContratoAbonado);
            var updatedContratoAbonado = _mapper.Map<ContratoAbonadoResponseDto>(dbContratoAbonado);
            return ApiResponse.Success("El abono ha sido actualizado.", updatedContratoAbonado);
        }

        public async Task<ApiResponse> Pay(int id, ContratoAbonadoPagarRequestDto abonado)
        {
            var dbContratoAbonado = await _contratoAbonadoRepository.GetById(id);

            if (dbContratoAbonado.FechaInicio > abonado.FechaPago)
                throw new BadRequestException("La fecha de pago no puede ser antes de la fecha de inicio.");

            if (dbContratoAbonado.EstadoPago)
                throw new BadRequestException("El abono ya está pagado.");

            var cajaChicaAbierta = await _cajaChicaRepository.GetByEstadoTrue() ??
                throw new BadRequestException("No es posible modificar el estado de pago del abono porque no hay ninguna caja chica abierta actualmente.");

            var fechaHoraPago = abonado.FechaPago.Date + abonado.HoraPago;
            var fechaHoraInicio = cajaChicaAbierta.FechaInicio.Date + cajaChicaAbierta.HoraInicio;

            if (fechaHoraInicio > fechaHoraPago)
                throw new BadRequestException($"La fecha y hora de pago del abono ({fechaHoraPago:yyyy-MM-dd HH:mm:ss}) no puede ser anterior a la fecha y hora de inicio de la caja chica abierta ({fechaHoraInicio:yyyy-MM-dd HH:mm:ss}).");

            dbContratoAbonado.IdCaja = cajaChicaAbierta.IdCaja;
            dbContratoAbonado.FechaPago = abonado.FechaPago;
            dbContratoAbonado.HoraPago = abonado.HoraPago;
            dbContratoAbonado.TipoPago = abonado.TipoPago;
            dbContratoAbonado.EstadoPago = true;

            await _contratoAbonadoRepository.Update(dbContratoAbonado);
            dbContratoAbonado = await _contratoAbonadoRepository.GetById(id);
            var abonoPagado = _mapper.Map<ContratoAbonadoResponseDto>(dbContratoAbonado);
            return ApiResponse.Success("El abono ha sido pagado.", abonoPagado);
        }

        public async Task<ApiResponse> CancelPayment(int id)
        {
            var dbContratoAbonado = await _contratoAbonadoRepository.GetById(id);

            if (!dbContratoAbonado.EstadoPago)
                throw new BadRequestException("El abono ya tiene el pago anulado.");

            bool hasClosedCajaChica = await _contratoAbonadoRepository.HasClosedCajaChicaById(id);
            if (hasClosedCajaChica)
                throw new BadRequestException("La caja chica en donde el abono está cerrada. Por lo tanto, no se puede modificar.");

            dbContratoAbonado.IdCaja = null;
            dbContratoAbonado.FechaPago = null;
            dbContratoAbonado.HoraPago = null;
            dbContratoAbonado.TipoPago = null;
            dbContratoAbonado.EstadoPago = false;

            await _contratoAbonadoRepository.Update(dbContratoAbonado);
            dbContratoAbonado = await _contratoAbonadoRepository.GetById(id);
            var abonoPagoAnulado = _mapper.Map<ContratoAbonadoResponseDto>(dbContratoAbonado);
            return ApiResponse.Success("El pago del abono ha sido anulado.", abonoPagoAnulado);
        }

        public async Task<ApiResponse> DeleteById(int id)
        {
            var dbContratoAbonado = await _contratoAbonadoRepository.GetById(id);

            if(dbContratoAbonado.EstadoPago)
            {
                bool hasClosedCajaChica = await _contratoAbonadoRepository.HasClosedCajaChicaById(id);
                if (hasClosedCajaChica)
                    throw new BadRequestException("La caja chica en donde el abono está cerrada. Por lo tanto, no se puede eliminar.");

                throw new BadRequestException("El abono está pagado, por lo que no es posible eliminarlo.");
            }

            await _contratoAbonadoRepository.Delete(dbContratoAbonado);
            return ApiResponse.Success("El abono ha sido eliminado.");
        }
    }
}
