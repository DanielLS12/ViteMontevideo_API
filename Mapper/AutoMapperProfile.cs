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
            CreateMap<CajaChicaCrearRequestDto, CajaChica>();
            CreateMap<CajaChica, CajaChicaResponseDto>();

            // Clientes
            CreateMap<Cliente, ClienteDto>();
            CreateMap<ClienteCrearRequestDto, Cliente>();
            CreateMap<Cliente, ClienteResponseDto>()
                    .ForMember(dest => dest.NumeroVehiculos, opt => opt.MapFrom(src => src.Vehiculos.Count))
                    .ForMember(dest => dest.NumeroComerciosAdicionales, opt => opt.MapFrom(src => src.ComerciosAdicinales.Count));

            // Comercios Adicionales
            CreateMap<ComercioAdicionalCrearRequestDto, ComercioAdicional>();
            CreateMap<ComercioAdicional, ComercioAdicionalResponseDto>();

            // Contratos Abonado
            CreateMap<ContratoAbonadoCrearRequestDto, ContratoAbonado>();
            CreateMap<ContratoAbonado, ContratoAbonadoResponseDto>();

            // Egresos
            CreateMap<EgresoCrearRequestDto, Egreso>();
            CreateMap<Egreso, EgresoResponseDto>();

            // Servicios
            CreateMap<Servicio, ServicioCrearRequestDto>();

            // Tarifas
            CreateMap<TarifaCrearRequestDto, Tarifa>();
            CreateMap<Tarifa,TarifaResponseDto>();

            // Trabajadores
            CreateMap<TrabajadorCrearRequestDto, Trabajador>();
            CreateMap<Trabajador, TrabajadorResponseDto>();

            // Usuarios
            CreateMap<Usuario, UsuarioDto>();

            // Vehiculos
            CreateMap<VehiculoCrearRequestDto, Vehiculo>();
            CreateMap<Vehiculo, VehiculoResponseDto>();
            CreateMap<Vehiculo, VehiculoSimplificadoDto>();
        }
    }
}
