using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Services.Exceptions;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Services.Interfaces;
using ViteMontevideo_API.Services.Dtos.Common;
using ViteMontevideo_API.Services.Dtos.Vehiculos.Parameters;
using ViteMontevideo_API.Services.Dtos.Vehiculos.Requests;
using ViteMontevideo_API.Services.Dtos.Vehiculos.Responses;

namespace ViteMontevideo_API.Services.Implementation
{
    public class VehiculoService : IVehiculoService
    {
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly ITarifaRepository _tarifaRepository;
        private readonly IMapper _mapper;

        public VehiculoService(
            IVehiculoRepository vehiculoRepository,
            IClienteRepository clienteRepository,
            ITarifaRepository tarifaRepository,
            IMapper mapper)
        {
            _vehiculoRepository = vehiculoRepository;
            _clienteRepository = clienteRepository;
            _tarifaRepository = tarifaRepository;
            _mapper = mapper;
        }

        public async Task<PageCursorResponse<VehiculoFullResponseDto>> GetAllPageCursor(FiltroVehiculo filtro)
        {
            var query = _vehiculoRepository.Query();

            if (!string.IsNullOrWhiteSpace(filtro.Placa) && filtro.Placa.Length >= 3)
                query = query.Where(v => v.Placa.Contains(filtro.Placa.ToUpper()));

            int cantidad = await query.CountAsync();

            query = _vehiculoRepository.ApplyPageCursor(query, filtro.Cursor, filtro.Count, v => v.IdVehiculo);

            var data = await query
                .OrderByDescending(v => v.IdVehiculo)
                .ProjectTo<VehiculoFullResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            int siguienteCursor = data.Any() ? data.LastOrDefault()?.IdVehiculo ?? 0 : 0;

            if (siguienteCursor == 0)
                cantidad = 0;

            return new PageCursorResponse<VehiculoFullResponseDto>(cantidad, siguienteCursor, data);
        }

        public async Task<VehiculoFullResponseDto> GetById(int id)
        {
            var vehiculo = await _vehiculoRepository.GetById(id)
                ?? throw new NotFoundException("Vehículo no encontrado.");

            return _mapper.Map<VehiculoFullResponseDto>(vehiculo);
        }

        public async Task<VehiculoFullResponseDto> GetByPlaca(string placa)
        {
            var vehiculo = await _vehiculoRepository.GetByPlaca(placa)
                ?? throw new NotFoundException("Vehículo no encontrado.");

            return _mapper.Map<VehiculoFullResponseDto>(vehiculo);
        }

        public async Task<ApiResponse> Insert(VehiculoCrearRequestDto vehiculo)
        {
            bool existsTarifa = await _tarifaRepository.ExistsById(vehiculo.IdTarifa);
            if (!existsTarifa)
                throw new BadRequestException("La tarifa que intento vincular al vehículo no existe.");

            if (vehiculo.IdCliente != null)
            {
                bool existsCliente = await _clienteRepository.ExistsById((int)vehiculo.IdCliente);
                if (!existsCliente)
                    throw new BadRequestException("El cliente que intento vincular al vehículo no existe.");
            }

            bool existsPlaca = await _vehiculoRepository.ExistsByPlaca(vehiculo.Placa);
            if (existsPlaca)
                throw new ConflictException("La placa ingresada ya existe en otro vehículo.");

            vehiculo.Placa = vehiculo.Placa.ToUpper();

            var dbVehiculo = _mapper.Map<Vehiculo>(vehiculo);
            dbVehiculo = await _vehiculoRepository.Insert(dbVehiculo);
            dbVehiculo = await _vehiculoRepository.GetById(dbVehiculo.IdVehiculo);
            var createdVehiculo = _mapper.Map<VehiculoFullResponseDto>(dbVehiculo);
            return ApiResponse.Success("El vehículo ha sido agregado.", createdVehiculo);
        }

        public async Task<ApiResponse> Update(int id, VehiculoActualizarRequestDto vehiculo)
        {
            bool existsTarifa = await _tarifaRepository.ExistsById(vehiculo.IdTarifa);
            if (!existsTarifa)
                throw new BadRequestException("La tarifa que intento vincular al vehículo no existe.");

            if (vehiculo.IdCliente != null)
            {
                bool existsCliente = await _clienteRepository.ExistsById((int)vehiculo.IdCliente);
                if (!existsCliente)
                    throw new BadRequestException("El cliente que intento vincular al vehículo no existe.");
            }

            var dbVehiculo = await _vehiculoRepository.GetById(id)
                ?? throw new NotFoundException("Vehículo no encontrado.");

            dbVehiculo.IdTarifa = vehiculo.IdTarifa;
            dbVehiculo.IdCliente = vehiculo.IdCliente;

            await _vehiculoRepository.Update(dbVehiculo);
            dbVehiculo = await _vehiculoRepository.GetById(id);
            var updatedVehiculo = _mapper.Map<VehiculoFullResponseDto>(dbVehiculo);
            return ApiResponse.Success("El vehículo ha sido actualizado.", updatedVehiculo);

        }

        public async Task<ApiResponse> DeleteById(int id)
        {
            var dbVehiculo = await _vehiculoRepository.GetById(id)
                ?? throw new NotFoundException("Vehículo no encontrado.");

            bool hasAbonados = await _vehiculoRepository.HasAbonadosById(id);
            bool hasServicios = await _vehiculoRepository.HasServiciosById(id);

            if (hasAbonados || hasServicios)
                throw new BadRequestException("No se puede eliminar este vehículo porque tiene abonado(s) y/o servicio(s) registrados.");

            await _vehiculoRepository.Delete(dbVehiculo);
            return ApiResponse.Success("El vehículo ha sido eliminado.");
        }
    }
}
