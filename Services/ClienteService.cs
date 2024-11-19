using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;
using ViteMontevideo_API.Exceptions;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Presentation.Dtos.Clientes;
using ViteMontevideo_API.Presentation.Dtos.Clientes.Filtros;
using ViteMontevideo_API.Presentation.Dtos.Common;
using ViteMontevideo_API.Services.Interfaces;

namespace ViteMontevideo_API.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repository;
        private readonly IMapper _mapper;

        public ClienteService(IClienteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CursorResponse<ClienteResponseDto>> GetAllPageCursor(FiltroCliente filtro)
        {
            const int MaxRegistros = 200;
            var query = _repository.Query();

            if (!string.IsNullOrWhiteSpace(filtro.NombreCompleto))
            {
                var terminos = filtro.NombreCompleto.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (var termino in terminos)
                {
                    var lowerTermino = termino.ToLower();
                    query = query.Where(c =>
                        EF.Functions.Like(c.Nombres.ToLower(), $"%{lowerTermino}%") ||
                        EF.Functions.Like(c.Apellidos.ToLower(), $"%{lowerTermino}%"));
                }
            }

            int cantidad = query.Count();

            query = _repository.ApplyPageCursor(query, filtro.Cursor, filtro.Count, MaxRegistros);

            var data = await query
                .OrderByDescending(c => c.IdCliente)
                .ProjectTo<ClienteResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            int siguienteCursor = data.Any() ? (data.LastOrDefault()?.IdCliente ?? 0) : 0;

            if(siguienteCursor == 0)
                cantidad = 0;

            return new CursorResponse<ClienteResponseDto>(cantidad, siguienteCursor, data);
        }

        public async Task<ClienteDto> GetById(int id)
        {
            var cliente = await _repository.GetById(id);
            return _mapper.Map<ClienteDto>(cliente);
        }

        public async Task<ApiResponse> Insert(ClienteCrearRequestDto cliente)
        {
            cliente = LimpiarDatos(cliente);
            var dbCliente = _mapper.Map<Cliente>(cliente);
            dbCliente = await _repository.Insert(dbCliente);
            var createdCliente = _mapper.Map<ClienteDto>(dbCliente);
            return ApiResponse.Success("El cliente ha sido creado.", createdCliente);
        }

        public async Task<ApiResponse> Update(int id, ClienteActualizarRequestDto cliente)
        {
            cliente = LimpiarDatos(cliente);

            var dbCliente = await _repository.GetById(id);

            dbCliente.Nombres = cliente.Nombres;
            dbCliente.Apellidos = cliente.Apellidos;
            dbCliente.Telefono = cliente.Telefono;
            dbCliente.Correo = cliente.Correo;

            dbCliente = await _repository.Update(dbCliente);
            var updatedCliente = _mapper.Map<ClienteDto>(dbCliente);
            return ApiResponse.Success("El cliente ha sido actualizado.", updatedCliente);
        }

        public async Task<ApiResponse> DeleteById(int id)
        {
            bool hasComerciosAdicionales = await _repository.HasComerciosAdicionalesById(id);
            bool hasVehiculos = await _repository.HasVehiculosById(id);

            if(hasComerciosAdicionales || hasVehiculos)
            {
                throw new BadRequestException("No se puede eliminar este cliente porque tiene vehículo(s) y/o comercio(s) adicional(es).");
            }

            await _repository.DeleteById(id);
            return ApiResponse.Success("El cliente ha sido eliminado.");
        }

        private static ClienteCrearRequestDto LimpiarDatos(ClienteCrearRequestDto cliente)
        {
            cliente.Nombres = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(cliente.Nombres, @"\s+", " ").Trim());
            cliente.Apellidos = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(cliente.Apellidos, @"\s+", " ").Trim());
            if (cliente.Telefono != null) cliente.Telefono = Regex.Replace(cliente.Telefono, @"\s+", " ").Trim();
            if (cliente.Correo != null) cliente.Correo = Regex.Replace(cliente.Correo, @"\s+", "").Trim();
            return cliente;
        }

        private static ClienteActualizarRequestDto LimpiarDatos(ClienteActualizarRequestDto cliente)
        {
            if (cliente.Nombres != null)
                cliente.Nombres = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(cliente.Nombres, @"\s+", " ").Trim());
            if (cliente.Apellidos != null)
                cliente.Apellidos = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(cliente.Apellidos, @"\s+", " ").Trim());
            if (cliente.Telefono != null)
                cliente.Telefono = Regex.Replace(cliente.Telefono, @"\s+", " ").Trim();
            if (cliente.Correo != null)
                cliente.Correo = Regex.Replace(cliente.Correo, @"\s+", "").Trim();
            return cliente;
        }
    }
}
