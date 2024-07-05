using AutoMapper;
using ViteMontevideo_API.Dtos.CajasChicas;
using ViteMontevideo_API.Dtos.Clientes;
using ViteMontevideo_API.Dtos.ComerciosAdicionales;
using ViteMontevideo_API.Dtos.ContratosAbonado;
using ViteMontevideo_API.Dtos.Egresos;
using ViteMontevideo_API.Dtos.Servicios;
using ViteMontevideo_API.Dtos.Tarifas;
using ViteMontevideo_API.Dtos.Trabajadores;
using ViteMontevideo_API.Dtos.Usuarios;
using ViteMontevideo_API.Dtos.Vehiculos;
using ViteMontevideo_API.models;
using ViteMontevideo_API.Models;

namespace ViteMontevideo_API.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            // Cajas Chicas
            CreateMap<CajaChicaRequestDto, CajaChica>();
            CreateMap<CajaChica, CajaChicaResponseDto>();

            // Clientes
            CreateMap<Cliente, ClienteDto>();
            CreateMap<ClienteRequestDto, Cliente>();
            CreateMap<Cliente, ClienteResponseDto>()
                    .ForMember(dest => dest.NumeroVehiculos, opt => opt.MapFrom(src => src.Vehiculos.Count))
                    .ForMember(dest => dest.NumeroComerciosAdicionales, opt => opt.MapFrom(src => src.ComerciosAdicinales.Count));

            // Comercios Adicionales
            CreateMap<ComercioAdicionalRequestDto, ComercioAdicional>();
            CreateMap<ComercioAdicional, ComercioAdicionalResponseDto>();

            // Contratos Abonado
            CreateMap<ContratoAbonadoRequestDto, ContratoAbonado>();
            CreateMap<ContratoAbonado, ContratoAbonadoResponseDto>();

            // Egresos
            CreateMap<EgresoRequestDto, Egreso>();
            CreateMap<Egreso, EgresoResponseDto>();

            // Servicios
            CreateMap<Servicio, ServicioRequestDto>();

            // Tarifas
            CreateMap<TarifaRequestDto, Tarifa>();
            CreateMap<Tarifa,TarifaResponseDto>();

            // Trabajadores
            CreateMap<TrabajadorRequestDto, Trabajador>();
            CreateMap<Trabajador, TrabajadorResponseDto>();

            // Usuarios
            CreateMap<Usuario, UsuarioDto>();

            // Vehiculos
            CreateMap<VehiculoRequestDto, Vehiculo>();
            CreateMap<Vehiculo, VehiculoResponseDto>();
            CreateMap<Vehiculo, VehiculoSimplificadoDto>();
        }
    }
}
