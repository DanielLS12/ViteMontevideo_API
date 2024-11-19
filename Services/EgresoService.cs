﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Exceptions;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Presentation.Dtos.Common;
using ViteMontevideo_API.Presentation.Dtos.Egresos;
using ViteMontevideo_API.Presentation.Dtos.Egresos.Filtros;
using ViteMontevideo_API.Services.Interfaces;

namespace ViteMontevideo_API.Services
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

        public async Task<CursorResponse<EgresoResponseDto>> GetAllPageCursor(FiltroEgreso filtro)
        {
            const int MaxRegistros = 200;
            var query = _egresoRepository.Query();

            query = query.Where(e => e.Fecha >= filtro.FechaInicio && e.Fecha <= filtro.FechaFinal);

            int cantidad = query.Count();

            query = _egresoRepository.ApplyPageCursor(query, filtro.Cursor, filtro.Count, MaxRegistros);

            var data = await query
                .OrderByDescending(e => e.IdEgreso)
                .ProjectTo<EgresoResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            int siguienteCursor = data.Any() ? (data.LastOrDefault()?.IdEgreso ?? 0) : 0;

            if (siguienteCursor == 0)
                cantidad = 0;

            return new CursorResponse<EgresoResponseDto>(cantidad, siguienteCursor, data);
        }

        public async Task<EgresoResponseDto> GetById(int id)
        {
            var egreso = await _egresoRepository.GetById(id);
            return _mapper.Map<EgresoResponseDto>(egreso);
        }

        public async Task<ApiResponse> Insert(EgresoCrearRequestDto egreso)
        {
            var cajaChicaAbierta = await _cajaChicaRepository.GetByEstadoTrue() 
                ?? throw new BadRequestException("No es posible registrar el egreso porque no hay ninguna caja chica abierta.");

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
            var cajaChica = await _cajaChicaRepository.GetById(egreso.IdCajaChica);
            if (!cajaChica.Estado)
                throw new BadRequestException("La caja chica en donde el egreso se encuentra esta cerrada. Por lo tanto, no se puede modificar.");

            var dbEgreso = await _egresoRepository.GetById(id);
            dbEgreso.Motivo = egreso.Motivo;
            dbEgreso.Monto = egreso.Monto;
            dbEgreso.Hora = egreso.Hora;

            await _egresoRepository.Update(dbEgreso);
            var updatedEgreso = _mapper.Map<EgresoResponseDto>(dbEgreso);
            return ApiResponse.Success("El egreso ha sido actualizado.", updatedEgreso);
        }

        public async Task<ApiResponse> DeleteById(int id)
        {
            var egreso = await _egresoRepository.GetById(id);
            var cajaChica = await _cajaChicaRepository.GetById(egreso.IdCaja);
            if(!cajaChica.Estado)
                throw new BadRequestException("La caja chica en donde el egreso se encuentra esta cerrada. Por lo tanto, no se puede eliminar.");

            await _egresoRepository.Delete(egreso);
            return ApiResponse.Success("La caja chica ha sido eliminada.");
        }
    }
}