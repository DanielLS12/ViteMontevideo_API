using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Services.Dtos.CajasChicas.Parameters;
using ViteMontevideo_API.Services.Dtos.CajasChicas.Requests;
using ViteMontevideo_API.Services.Dtos.CajasChicas.Responses;
using ViteMontevideo_API.Services.Dtos.Common;
using ViteMontevideo_API.Services.Exceptions;
using ViteMontevideo_API.Services.Interfaces;

namespace ViteMontevideo_API.Services.Implementation
{
    public class CajaChicaService : ICajaChicaService
    {
        private readonly ICajaChicaRepository _cajaChicaRepository;
        private readonly ITrabajadorRepository _trabajadorRepository;
        private readonly IMapper _mapper;

        public CajaChicaService(ICajaChicaRepository cajaChicaRepository, ITrabajadorRepository trabajadorRepository, IMapper mapper)
        {
            _cajaChicaRepository = cajaChicaRepository;
            _trabajadorRepository = trabajadorRepository;
            _mapper = mapper;
        }

        public async Task<PageCursorResponse<CajaChicaResponseDto>> GetAllPageCursor(FiltroCajaChica filtro)
        {
            var query = _cajaChicaRepository.Query();

            query = query.Where(e => e.FechaInicio >= filtro.FechaInicio && e.FechaInicio <= filtro.FechaFinal);

            if (filtro.Turno.HasValue)
                query = query.Where(c => c.Turno.Contains(filtro.Turno.Value.ToString()));

            int cantidad = await query.CountAsync();

            query = _cajaChicaRepository.ApplyPageCursor(query, filtro.Cursor, filtro.Count, e => e.IdCaja);

            var data = await query
                .OrderByDescending(e => e.IdCaja)
                .ProjectTo<CajaChicaResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            int siguienteCursor = data.Any() ? data.LastOrDefault()?.IdCaja ?? 0 : 0;

            if (siguienteCursor == 0)
                cantidad = 0;

            return new PageCursorResponse<CajaChicaResponseDto>(cantidad, siguienteCursor, data);
        }

        public async Task<List<InformeCajaChica>> GetAllInformes(DateTime fecha) =>
            await _cajaChicaRepository.GetAllInformes(fecha);

        public async Task<CajaChicaResponseDto> GetById(int id)
        {
            var cajaChica = await _cajaChicaRepository.GetById(id)
                ?? throw new NotFoundException("Caja chica no encontrada.");

            return _mapper.Map<CajaChicaResponseDto>(cajaChica);
        }

        public async Task<ApiResponse> Insert(CajaChicaCrearRequestDto cajaChicaDto)
        {
            var trabajador = await _trabajadorRepository.GetById(cajaChicaDto.IdTrabajador)
                ?? throw new BadRequestException("El trabajador que intento vincular a la caja chica no existe.");

            if (!trabajador.Estado)
                throw new BadRequestException("El trabajador seleccionado no esta disponible.");

            bool existsOpenCajaChica = await _cajaChicaRepository.ExistsOpenCajaChica();
            if (existsOpenCajaChica)
                throw new BadRequestException("No se puede crear o abrir otra caja chica.");

            var dbCajaChica = _mapper.Map<CajaChica>(cajaChicaDto);
            dbCajaChica.Estado = true;
            dbCajaChica = await _cajaChicaRepository.Insert(dbCajaChica);
            var createdCajaChica = _mapper.Map<CajaChicaResponseDto>(dbCajaChica);
            return ApiResponse.Success("La caja chica ha sido creada.", createdCajaChica);
        }

        public async Task<ApiResponse> Update(int id, CajaChicaActualizarRequestDto cajaChicaDto)
        {
            var trabajador = await _trabajadorRepository.GetById(cajaChicaDto.IdTrabajador)
                ?? throw new BadRequestException("El trabajador que intento vincular a la caja chica no existe.");

            var dbCajaChica = await _cajaChicaRepository.GetById(id)
                ?? throw new NotFoundException("Caja chica no encontrada.");

            dbCajaChica.IdTrabajador = cajaChicaDto.IdTrabajador;
            dbCajaChica.Turno = cajaChicaDto.Turno;
            dbCajaChica.SaldoInicial = cajaChicaDto.SaldoInicial;
            dbCajaChica.Observacion = string.IsNullOrWhiteSpace(cajaChicaDto.Observacion) ? null : cajaChicaDto.Observacion.Trim();

            await _cajaChicaRepository.Update(dbCajaChica);
            dbCajaChica.Trabajador = trabajador;
            var updatedCajaChica = _mapper.Map<CajaChicaResponseDto>(dbCajaChica);
            return ApiResponse.Success("La caja chica ha sido actualizada.", updatedCajaChica);
        }

        public async Task<ApiResponse> Open(int id)
        {
            var dbCajaChica = await _cajaChicaRepository.GetById(id)
                ?? throw new NotFoundException("Caja chica no encontrada.");

            if (dbCajaChica.Estado)
                throw new BadRequestException("La caja chica ya estaba abierta.");

            var existsOpenCajaChica = await _cajaChicaRepository.ExistsOpenCajaChica();
            if (existsOpenCajaChica)
                throw new BadRequestException("Ya existe una caja chica abierta.");

            dbCajaChica.FechaFinal = null;
            dbCajaChica.HoraFinal = null;
            dbCajaChica.Estado = true;

            await _cajaChicaRepository.Update(dbCajaChica);
            dbCajaChica = await _cajaChicaRepository.GetById(id);
            var openedCajaChica = _mapper.Map<CajaChicaResponseDto>(dbCajaChica);
            return ApiResponse.Success("La caja chica ha sido abierta.", openedCajaChica);
        }

        public async Task<ApiResponse> Close(int id, CajaChicaCerrarRequestDto cajaChicaDto)
        {
            var dbCajaChica = await _cajaChicaRepository.GetById(id)
                ?? throw new NotFoundException("Caja chica no encontrada.");

            var fechaHoraInicio = dbCajaChica.FechaInicio.Date + dbCajaChica.HoraInicio;
            var fechaHoraFinal = cajaChicaDto.FechaFinal.Date + cajaChicaDto.HoraFinal;

            if (fechaHoraInicio > fechaHoraFinal)
                throw new BadRequestException($"La fecha y hora final ({fechaHoraFinal:yyyy-MM-dd HH:mm:ss}) no puede ser anterior a la fecha y hora de inicio ({fechaHoraInicio:yyyy-MM-dd HH:mm:ss}).");

            if (!dbCajaChica.Estado)
                throw new BadRequestException("La caja chica ya estaba cerrado.");

            dbCajaChica.FechaFinal = cajaChicaDto.FechaFinal;
            dbCajaChica.HoraFinal = cajaChicaDto.HoraFinal;
            dbCajaChica.Observacion = string.IsNullOrWhiteSpace(cajaChicaDto.Observacion) ? null : cajaChicaDto.Observacion.Trim();
            dbCajaChica.Estado = false;

            await _cajaChicaRepository.Update(dbCajaChica);
            dbCajaChica = await _cajaChicaRepository.GetById(id);
            var closedCajaChica = _mapper.Map<CajaChicaResponseDto>(dbCajaChica);
            return ApiResponse.Success("La caja chica ha sido cerrada.", closedCajaChica);
        }

        public async Task<ApiResponse> DeleteById(int id)
        {
            var dbCajaChica = await _cajaChicaRepository.GetById(id)
                ?? throw new NotFoundException("Caja chica no encontrada.");

            bool hasRelations = await _cajaChicaRepository.HasContratosAbonadosOrEgresosOrServicios(id);
            if (hasRelations)
                throw new BadRequestException("No se puede eliminar esta caja porque contiene egresos, abonados o servicios.");

            await _cajaChicaRepository.Delete(dbCajaChica);
            return ApiResponse.Success("La caja chica ha sido eliminada.");
        }
    }
}
