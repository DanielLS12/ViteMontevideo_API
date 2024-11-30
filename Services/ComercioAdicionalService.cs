using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Shared.Exceptions;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Presentation.Dtos.ComerciosAdicionales;
using ViteMontevideo_API.Presentation.Dtos.ComerciosAdicionales.Filtros;
using ViteMontevideo_API.Presentation.Dtos.Common;
using ViteMontevideo_API.Services.Interfaces;

namespace ViteMontevideo_API.Services
{
    public class ComercioAdicionalService : IComercioAdicionalService
    {
        private readonly IComercioAdicionalRepository _comercioAdicionalRepository;
        private readonly ICajaChicaRepository _cajaChicaRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public ComercioAdicionalService(
            IComercioAdicionalRepository comercioAdicionalRepository, 
            ICajaChicaRepository cajaChicaRepository, 
            IClienteRepository clienteRepository,
            IMapper mapper)
        {
            _comercioAdicionalRepository = comercioAdicionalRepository;
            _cajaChicaRepository = cajaChicaRepository;
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        public async Task<PageCursorMontoResponse<ComercioAdicionalResponseDto>> GetAllPageCursor(FiltroComercioAdicional filtro)
        {
            var query = _comercioAdicionalRepository.Query();

            if(!string.IsNullOrWhiteSpace(filtro.Cliente)) 
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
                query = query.Where(cad => cad.TipoComercioAdicional == filtro.Tipo.Value.ToString());

            if (filtro.TipoPago.HasValue)
                query = query.Where(cad => cad.TipoPago == filtro.TipoPago.Value.ToString());

            if (filtro.EstadoPago.HasValue)
            {
                if (filtro.TipoPago.HasValue && filtro.EstadoPago.Value)
                    query = query.Where(cad => cad.TipoPago == filtro.TipoPago.Value.ToString());
                query = query.Where(cad => filtro.EstadoPago.Value ? cad.IdCaja != null : cad.IdCaja == null);
            }

            int cantidad = await query.CountAsync();
            decimal totalMonto = await query.SumAsync(cad => cad.Monto);

            query = _comercioAdicionalRepository.ApplyPageCursor(query, filtro.Cursor, filtro.Count, cad => cad.IdComercioAdicional);

            var data = await query
                .OrderByDescending(cad => cad.IdComercioAdicional)
                .ProjectTo<ComercioAdicionalResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            int siguienteCursor = data.Any() ? (data.LastOrDefault()?.IdComercioAdicional ?? 0) : 0;

            if (siguienteCursor == 0)
                cantidad = 0;

            return new PageCursorMontoResponse<ComercioAdicionalResponseDto>(cantidad, siguienteCursor, data, totalMonto);

        }

        public async Task<ComercioAdicionalResponseDto> GetById(int id)
        {
            var comercioAdicional = await _comercioAdicionalRepository.GetById(id)
                ?? throw new NotFoundException("Servicio adicional no encontrado.");

            return _mapper.Map<ComercioAdicionalResponseDto>(comercioAdicional);
        }

        public async Task<ApiResponse> Insert(ComercioAdicionalCrearRequestDto comercioAdicional)
        {
            bool existsCliente = await _clienteRepository.ExistsById(comercioAdicional.IdCliente);
            if (!existsCliente)
                throw new BadRequestException("El cliente que intento vincular al servicio adicional no existe.");

            var dbComercioAdicional = _mapper.Map<ComercioAdicional>(comercioAdicional);
            dbComercioAdicional = await _comercioAdicionalRepository.Insert(dbComercioAdicional);
            dbComercioAdicional = await _comercioAdicionalRepository.GetById(dbComercioAdicional.IdComercioAdicional);
            var createdComercioAdicional = _mapper.Map<ComercioAdicionalResponseDto>(dbComercioAdicional);
            return ApiResponse.Success("El servicio adicional ha sido creado.", createdComercioAdicional);
        }

        public async Task<ApiResponse> Update(int id, ComercioAdicionalActualizarRequestDto comercioAdicional)
        {
            bool existsCliente = await _clienteRepository.ExistsById(comercioAdicional.IdCliente);
            if (!existsCliente)
                throw new BadRequestException("El cliente que intento vincular al servicio adicional no existe.");

            var dbComercioAdicional = await _comercioAdicionalRepository.GetById(id) 
                ?? throw new NotFoundException("Servicio adicional no encontrado.");

            if(dbComercioAdicional.IdCaja.HasValue)
                throw new BadRequestException("El servicio adicional está pagado, por lo que no es posible actualizarlo.");

            dbComercioAdicional.IdCliente = comercioAdicional.IdCliente;
            dbComercioAdicional.TipoComercioAdicional = comercioAdicional.TipoComercioAdicional;
            dbComercioAdicional.Monto = comercioAdicional.Monto;
            dbComercioAdicional.Observacion = string.IsNullOrWhiteSpace(comercioAdicional.Observacion) ? null : comercioAdicional.Observacion.Trim();

            await _comercioAdicionalRepository.Update(dbComercioAdicional);
            dbComercioAdicional = await _comercioAdicionalRepository.GetById(id);
            var updatedComercioAdicional = _mapper.Map<ComercioAdicionalResponseDto>(dbComercioAdicional);
            return ApiResponse.Success("El servicio adicional ha sido actualizado.", updatedComercioAdicional);
        }

        public async Task<ApiResponse> Pay(int id, ComercioAdicionalPagarRequestDto comercioAdicional)
        {
            var dbComercioAidicional = await _comercioAdicionalRepository.GetById(id) 
                ?? throw new NotFoundException("Servicio adicional no encontrado.");

            if (dbComercioAidicional.IdCaja.HasValue)
                throw new BadRequestException("El servicio adicional ya está pagado.");

            var cajaChicaAbierta = await _cajaChicaRepository.GetOpenCajaChica() ??
                throw new BadRequestException("No es posible actualizar el estado de pago del servicio adicional porque no hay ninguna caja chica abierta actualmente.");

            var fechaHoraPago = comercioAdicional.FechaPago.Date + comercioAdicional.HoraPago;
            var fechaHoraInicio = cajaChicaAbierta.FechaInicio.Date + cajaChicaAbierta.HoraInicio;

            if (fechaHoraInicio > fechaHoraPago)
                throw new BadRequestException($"La fecha y hora de pago del servicio adicional ({fechaHoraPago:yyyy-MM-dd HH:mm:ss}) no puede ser anterior a la fecha y hora de inicio de la caja chica abierta ({fechaHoraInicio:yyyy-MM-dd HH:mm:ss}).");

            dbComercioAidicional.IdCaja = cajaChicaAbierta.IdCaja;
            dbComercioAidicional.FechaPago = comercioAdicional.FechaPago;
            dbComercioAidicional.HoraPago = comercioAdicional.HoraPago;
            dbComercioAidicional.TipoPago = comercioAdicional.TipoPago;

            await _comercioAdicionalRepository.Update(dbComercioAidicional);
            dbComercioAidicional = await _comercioAdicionalRepository.GetById(id);
            var comercioAdicionalPagado = _mapper.Map<ComercioAdicionalResponseDto>(dbComercioAidicional);
            return ApiResponse.Success("El servicio adicional ha sido pagado.", comercioAdicionalPagado);
        }

        public async Task<ApiResponse> CancelPayment(int id)
        {
            var dbComercioAdicional = await _comercioAdicionalRepository.GetById(id)
                ?? throw new NotFoundException("Servicio adicional no encontrado.");

            if (!dbComercioAdicional.IdCaja.HasValue)
                throw new BadRequestException("El servicio adicional ya tiene el pago anulado.");

            bool isCajaChicaClosed = await _cajaChicaRepository.IsCajaChicaClosedById(dbComercioAdicional.IdCaja.Value);
            if (isCajaChicaClosed)
                throw new BadRequestException("La caja chica que contiene el servicio adicional está cerrada, por lo que no se puede anular el pago.");

            dbComercioAdicional.IdCaja = null;
            dbComercioAdicional.FechaPago = null;
            dbComercioAdicional.HoraPago = null;
            dbComercioAdicional.TipoPago = null;

            await _comercioAdicionalRepository.Update(dbComercioAdicional);
            dbComercioAdicional = await _comercioAdicionalRepository.GetById(id);
            var comercioAdicionalPagoCancelado = _mapper.Map<ComercioAdicionalResponseDto>(dbComercioAdicional);
            return ApiResponse.Success("El pago del servicio adicional ha sido anulado.", comercioAdicionalPagoCancelado);
        }

        public async Task<ApiResponse> DeleteById(int id)
        {
            var dbComercioAdicional = await _comercioAdicionalRepository.GetById(id) 
                ?? throw new NotFoundException("Servicio adicional no encontrado.");

            if(dbComercioAdicional.IdCaja.HasValue)
                throw new BadRequestException("El servicio adicional está pagado, por lo que no es posible eliminarlo.");

            await _comercioAdicionalRepository.Delete(dbComercioAdicional);
            return ApiResponse.Success("El servicio adicional ha sido eliminado.");
        }
    }
}
