using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Services.Exceptions;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Services.Interfaces;
using ViteMontevideo_API.Services.Dtos.Common;
using ViteMontevideo_API.Services.Dtos.Egresos.Parameters;
using ViteMontevideo_API.Services.Dtos.Egresos.Requests;
using ViteMontevideo_API.Services.Dtos.Egresos.Responses;

namespace ViteMontevideo_API.Services.Implementation
{
    public class EgresoService : IEgresoService
    {
        private readonly IEgresoRepository _egresoRepository;
        private readonly ICajaChicaRepository _cajaChicaRepository;
        private readonly IMapper _mapper;

        public EgresoService(IEgresoRepository egresoRepository, ICajaChicaRepository cajaChicaRepository, IMapper mapper)
        {
            _egresoRepository = egresoRepository;
            _cajaChicaRepository = cajaChicaRepository;
            _mapper = mapper;
        }

        public async Task<PageCursorMontoResponse<EgresoResponseDto>> GetAllPageCursor(FiltroEgreso filtro)
        {
            var query = _egresoRepository.Query();

            query = query.Where(e => e.Fecha >= filtro.FechaInicio && e.Fecha <= filtro.FechaFinal);

            int cantidad = query.Count();
            decimal totalMonto = query.Sum(e => e.Monto);

            query = _egresoRepository.ApplyPageCursor(query, filtro.Cursor, filtro.Count, e => e.IdEgreso);

            var data = await query
                .OrderByDescending(e => e.IdEgreso)
                .ProjectTo<EgresoResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            int siguienteCursor = data.Any() ? data.LastOrDefault()?.IdEgreso ?? 0 : 0;

            if (siguienteCursor == 0)
                cantidad = 0;

            return new PageCursorMontoResponse<EgresoResponseDto>(cantidad, siguienteCursor, data, totalMonto);
        }

        public async Task<EgresoResponseDto> GetById(int id)
        {
            var egreso = await _egresoRepository.GetById(id)
                ?? throw new NotFoundException("Egreso no encontrado.");

            return _mapper.Map<EgresoResponseDto>(egreso);
        }

        public async Task<ApiResponse> Insert(EgresoCrearRequestDto egreso)
        {
            var cajaChicaAbierta = await _cajaChicaRepository.GetOpenCajaChica()
                ?? throw new BadRequestException("No es posible registrar el egreso porque no hay ninguna caja chica abierta actualmente.");

            if (cajaChicaAbierta.FechaInicio > egreso.Fecha)
                throw new BadRequestException($"La fecha del egreso debe ser igual o mayor a la fecha de inicio ({cajaChicaAbierta.FechaInicio.ToShortDateString()}) de la caja chica abierta encontrada.");

            var dbEgreso = _mapper.Map<Egreso>(egreso);
            dbEgreso.IdCaja = cajaChicaAbierta.IdCaja;
            dbEgreso = await _egresoRepository.Insert(dbEgreso);
            var createdEgreso = _mapper.Map<EgresoResponseDto>(dbEgreso);
            return ApiResponse.Success("El egreso ha sido creado.", createdEgreso);
        }

        public async Task<ApiResponse> Update(int id, EgresoActualizarRequestDto egreso)
        {
            var dbEgreso = await _egresoRepository.GetById(id)
                ?? throw new NotFoundException("Egreso no encontrado."); ;

            bool isCajaChicaClosed = await _cajaChicaRepository.IsCajaChicaClosedById(dbEgreso.IdCaja);
            if (isCajaChicaClosed)
                throw new BadRequestException("La caja chica que contiene el egreso está cerrada, por lo que no se puede actualizar.");

            dbEgreso.Motivo = egreso.Motivo;
            dbEgreso.Monto = egreso.Monto;
            dbEgreso.Hora = egreso.Hora;

            await _egresoRepository.Update(dbEgreso);
            var updatedEgreso = _mapper.Map<EgresoResponseDto>(dbEgreso);
            return ApiResponse.Success("El egreso ha sido actualizado.", updatedEgreso);
        }

        public async Task<ApiResponse> DeleteById(int id)
        {
            var dbEgreso = await _egresoRepository.GetById(id)
                ?? throw new NotFoundException("Egreso no encontrado.");

            bool isCajaChicaClosed = await _cajaChicaRepository.IsCajaChicaClosedById(dbEgreso.IdCaja);
            if (isCajaChicaClosed)
                throw new BadRequestException("La caja chica que contiene el egreso está cerrada, por lo que no se puede eliminar.");

            await _egresoRepository.Delete(dbEgreso);
            return ApiResponse.Success("La caja chica ha sido eliminada.");
        }
    }
}
