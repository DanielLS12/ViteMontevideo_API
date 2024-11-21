using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Components.Forms;
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
            const int MaxRegistros = 200;
            var query = _contratoAbonadoRepository.Query();

            if (!string.IsNullOrWhiteSpace(filtro.Placa) && filtro.Placa.Length >= 3)
                query = query.Where(ca => ca.Vehiculo.Placa.Contains(filtro.Placa));

            if (filtro.EstaPagado != null)
                query = query.Where(ca => ca.EstadoPago == filtro.EstaPagado);

            int cantidad = await query.CountAsync();
            decimal totalMonto = query.Sum(ca => ca.Monto);

            query = _contratoAbonadoRepository.ApplyPageCursor(query, filtro.Cursor, filtro.Count, MaxRegistros, ca => ca.IdContratoAbonado);

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
            bool hasOpenCajaChica = await _contratoAbonadoRepository.HasOpenCajaChicaById(id);
            if (!hasOpenCajaChica)
                throw new BadRequestException("La caja chica en donde el abono se encuentra esta cerrada. Por lo tanto, no se puede modificar.");

            var dbContratoAbonado = await _contratoAbonadoRepository.GetById(id);
            if (dbContratoAbonado.EstadoPago)
                throw new BadRequestException("El abono ya ha sido pagado, por lo que no es posible modificarlo.");

            bool existsVehiculo = await _vehiculoRepository.ExistsById(abonado.IdVehiculo);
            if(!existsVehiculo)
                throw new BadRequestException("El vehículo que intento vincular al abono no existe.");

            dbContratoAbonado.IdVehiculo = abonado.IdVehiculo;
            dbContratoAbonado.FechaInicio = abonado.FechaInicio;
            dbContratoAbonado.FechaFinal = abonado.FechaFinal;
            dbContratoAbonado.HoraInicio = abonado.HoraInicio;
            dbContratoAbonado.HoraFinal = abonado.HoraFinal;
            dbContratoAbonado.Monto = abonado.Monto;
            dbContratoAbonado.Observacion = string.IsNullOrWhiteSpace(abonado.Observacion) ? null : abonado.Observacion.Trim();
            
            await _contratoAbonadoRepository.Update(dbContratoAbonado);
            dbContratoAbonado = await _contratoAbonadoRepository.GetById(id);
            var updatedContratoAbonado = _mapper.Map<ContratoAbonadoResponseDto>(dbContratoAbonado);
            return ApiResponse.Success("El abono ha sido actualizado.", updatedContratoAbonado);
        }

        public async Task<ApiResponse> Pay(int id, ContratoAbonadoPagarRequestDto abonado)
        {
            var dbContratoAbonado = await _contratoAbonadoRepository.GetById(id);
            if (dbContratoAbonado.EstadoPago)
                throw new BadRequestException("El abono ya estaba pagado.");

            var cajaChicaAbierta = await _cajaChicaRepository.GetByEstadoTrue()
                ?? throw new BadRequestException("No es posible modificar el estado de pago del abono porque no hay ninguna caja chica abierta actualmente.");

            dbContratoAbonado.IdCaja = cajaChicaAbierta.IdCaja;
            dbContratoAbonado.FechaPago = abonado.FechaPago;
            dbContratoAbonado.HoraPago = abonado.HoraPago;
            dbContratoAbonado.TipoPago = abonado.TipoPago;
            dbContratoAbonado.EstadoPago = true;

            await _contratoAbonadoRepository.Update(dbContratoAbonado);
            dbContratoAbonado = await _contratoAbonadoRepository.GetById(id);
            var paidContratoAbonado = _mapper.Map<ContratoAbonadoResponseDto>(dbContratoAbonado);
            return ApiResponse.Success("El abono ha sido pagado.", paidContratoAbonado);
        }

        public async Task<ApiResponse> CancelPayment(int id)
        {
            bool hasOpenCajaChica = await _contratoAbonadoRepository.HasOpenCajaChicaById(id);
            if (!hasOpenCajaChica)
                throw new BadRequestException("La caja chica en donde el abono se encuentra esta cerrada. Por lo tanto, no se puede modificar.");

            var dbContratoAbonado = await _contratoAbonadoRepository.GetById(id);
            if (!dbContratoAbonado.EstadoPago)
                throw new BadRequestException("El pago del abono ya estaba anulado.");

            dbContratoAbonado.IdCaja = null;
            dbContratoAbonado.FechaPago = null;
            dbContratoAbonado.HoraPago = null;
            dbContratoAbonado.TipoPago = null;
            dbContratoAbonado.EstadoPago = false;

            await _contratoAbonadoRepository.Update(dbContratoAbonado);
            dbContratoAbonado = await _contratoAbonadoRepository.GetById(id);
            var cancelPaidAbonado = _mapper.Map<ContratoAbonadoResponseDto>(dbContratoAbonado);
            return ApiResponse.Success("El pago del abono ha sido anulado.",cancelPaidAbonado);
        }

        public async Task<ApiResponse> DeleteById(int id)
        {
            bool hasOpenCajaChica = await _contratoAbonadoRepository.HasOpenCajaChicaById(id);
            if(!hasOpenCajaChica)
                throw new BadRequestException("La caja chica en donde el abono se encuentra esta cerrada. Por lo tanto, no se puede eliminar.");

            await _contratoAbonadoRepository.DeleteById(id);
            return ApiResponse.Success("El abono ha sido eliminado.");
        }
    }
}
