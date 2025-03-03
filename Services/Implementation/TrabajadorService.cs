﻿using AutoMapper;
using System.Globalization;
using System.Text.RegularExpressions;
using ViteMontevideo_API.Services.Exceptions;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Services.Interfaces;
using ViteMontevideo_API.Services.Dtos.Common;
using ViteMontevideo_API.Services.Dtos.Trabajadores.Requests;
using ViteMontevideo_API.Services.Dtos.Trabajadores.Responses;

namespace ViteMontevideo_API.Services.Implementation
{
    public class TrabajadorService : ITrabajadorService
    {
        public readonly ITrabajadorRepository _trabajadorRepository;
        public readonly ICargoRepository _cargoRepository;
        public readonly IMapper _mapper;

        public TrabajadorService(ITrabajadorRepository trabajadorRepository, ICargoRepository cargoRepository, IMapper mapper)
        {
            _trabajadorRepository = trabajadorRepository;
            _cargoRepository = cargoRepository;
            _mapper = mapper;
        }

        public async Task<DataResponse<TrabajadorResponseDto>> GetAll()
        {
            var trabajadores = await _trabajadorRepository.GetAll();
            trabajadores = trabajadores.OrderByDescending(t => t.IdTrabajador).ToList();
            int cantidad = trabajadores.Count();
            var data = _mapper.Map<List<TrabajadorResponseDto>>(trabajadores);
            return new DataResponse<TrabajadorResponseDto>(cantidad, data);
        }

        public async Task<TrabajadorResponseDto> GetById(short id)
        {
            var trabajador = await _trabajadorRepository.GetById(id)
                ?? throw new NotFoundException("Trabajador no encontrado.");

            return _mapper.Map<TrabajadorResponseDto>(trabajador);
        }

        public async Task<ApiResponse> Insert(TrabajadorCrearRequestDto trabajador)
        {
            trabajador = LimpiarDatos(trabajador);

            bool existsCargo = await _cargoRepository.ExistsById(trabajador.IdCargo);
            if (!existsCargo)
                throw new BadRequestException("No es posible registrar al trabajador porque no existe el cargo que se desea asignarle.");

            bool existsDni = await _trabajadorRepository.Exists(dni: trabajador.Dni);
            if (existsDni)
                throw new ConflictException("El dni ingresado ya se encuentra registrado.");

            bool existsTelefono = await _trabajadorRepository.Exists(telefono: trabajador.Telefono);
            if (existsTelefono)
                throw new ConflictException("El teléfono ingresado ya se encuentra registrado.");

            bool existsCorreo = await _trabajadorRepository.Exists(correo: trabajador.Correo);
            if (existsCorreo)
                throw new ConflictException("El correo ingresado ya se encuentra registrado.");

            var dbTrabajador = _mapper.Map<Trabajador>(trabajador);
            dbTrabajador.Estado = true;

            dbTrabajador = await _trabajadorRepository.Insert(dbTrabajador);
            dbTrabajador = await _trabajadorRepository.GetById(dbTrabajador.IdTrabajador);
            var createdTrabajador = _mapper.Map<TrabajadorResponseDto>(dbTrabajador);
            return ApiResponse.Success("El trabajador ha sido agregado.", createdTrabajador);
        }

        public async Task<ApiResponse> Update(short id, TrabajadorActualizarRequestDto trabajador)
        {
            trabajador = LimpiarDatos(trabajador);

            bool existsCargo = await _cargoRepository.ExistsById(trabajador.IdCargo);
            if (!existsCargo)
                throw new BadRequestException("No es posible registrar al trabajador porque no existe el cargo que se desea asignarle.");

            bool existsDni = await _trabajadorRepository.Exists(id, dni: trabajador.Dni);
            if (existsDni)
                throw new ConflictException("El dni ingresado ya se encuentra registrado.");

            bool existsTelefono = await _trabajadorRepository.Exists(id, telefono: trabajador.Telefono);
            if (existsTelefono)
                throw new ConflictException("El teléfono ingresado ya se encuentra registrado.");

            bool existsCorreo = await _trabajadorRepository.Exists(id, correo: trabajador.Correo);
            if (existsCorreo)
                throw new ConflictException("El correo ingresado ya se encuentra registrado.");

            var dbTrabajador = await _trabajadorRepository.GetById(id)
                ?? throw new NotFoundException("Trabajador no encontrado.");

            dbTrabajador.IdCargo = trabajador.IdCargo;
            dbTrabajador.Nombre = trabajador.Nombre;
            dbTrabajador.ApellidoPaterno = trabajador.ApellidoPaterno;
            dbTrabajador.ApellidoMaterno = trabajador.ApellidoMaterno;
            dbTrabajador.Dni = trabajador.Dni;
            dbTrabajador.Correo = trabajador.Correo;
            dbTrabajador.Telefono = trabajador.Telefono;
            dbTrabajador.Estado = trabajador.Estado;

            await _trabajadorRepository.Update(dbTrabajador);
            dbTrabajador = await _trabajadorRepository.GetById(id);
            var createdTrabajador = _mapper.Map<TrabajadorResponseDto>(dbTrabajador);
            return ApiResponse.Success("El trabajador ha sido actualizado.", createdTrabajador);
        }

        public async Task<ApiResponse> DeleteById(short id)
        {
            var dbTrabajador = await _trabajadorRepository.GetById(id)
                ?? throw new NotFoundException("Trabajador no encontrado.");

            var hasCajasChicas = await _trabajadorRepository.HasCajasChicasById(id);
            if (hasCajasChicas)
                throw new BadRequestException("No se puede eliminar este trabajador porque tiene cajas chicas registradas a su nombre.");

            await _trabajadorRepository.Delete(dbTrabajador);
            return ApiResponse.Success("El trabajador ha sido eliminado.");
        }

        private static TrabajadorCrearRequestDto LimpiarDatos(TrabajadorCrearRequestDto trabajador)
        {
            trabajador.Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(trabajador.Nombre, @"\s+", " ").Trim());
            trabajador.ApellidoPaterno = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(trabajador.ApellidoPaterno, @"\s+", " ").Trim());
            trabajador.ApellidoMaterno = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(trabajador.ApellidoMaterno, @"\s+", " ").Trim());

            if (trabajador.Telefono != null)
                trabajador.Telefono = Regex.Replace(trabajador.Telefono, @"\s+", " ");

            if (trabajador.Correo != null)
                trabajador.Correo = Regex.Replace(trabajador.Correo, @"\s+", "").Trim();

            return trabajador;
        }

        private static TrabajadorActualizarRequestDto LimpiarDatos(TrabajadorActualizarRequestDto trabajador)
        {
            if (trabajador.Nombre != null)
                trabajador.Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(trabajador.Nombre, @"\s+", " ").Trim());

            if (trabajador.ApellidoPaterno != null)
                trabajador.ApellidoPaterno = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(trabajador.ApellidoPaterno, @"\s+", " ").Trim());

            if (trabajador.ApellidoMaterno != null)
                trabajador.ApellidoMaterno = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(trabajador.ApellidoMaterno, @"\s+", " ").Trim());

            if (trabajador.Telefono != null)
                trabajador.Telefono = Regex.Replace(trabajador.Telefono, @"\s+", " ");

            if (trabajador.Correo != null)
                trabajador.Correo = Regex.Replace(trabajador.Correo, @"\s+", "").Trim();

            return trabajador;
        }
    }
}
