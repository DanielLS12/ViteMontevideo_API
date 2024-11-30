using AutoMapper;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Presentation.Dtos.CajasChicas;
using ViteMontevideo_API.Presentation.Dtos.Clientes;
using ViteMontevideo_API.Presentation.Dtos.ComerciosAdicionales;
using ViteMontevideo_API.Presentation.Dtos.ContratosAbonado;
using ViteMontevideo_API.Presentation.Dtos.Egresos;
using ViteMontevideo_API.Presentation.Dtos.Servicios;
using ViteMontevideo_API.Presentation.Dtos.Tarifas;
using ViteMontevideo_API.Presentation.Dtos.Trabajadores;
using ViteMontevideo_API.Presentation.Dtos.Usuarios;
using ViteMontevideo_API.Presentation.Dtos.Vehiculos;

namespace ViteMontevideo_API.Services.Mapper
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
            CreateMap<ServicioCrearRequestDto, Servicio>();
            CreateMap<Servicio, ServicioSalidaResponseDto>();
            CreateMap<Servicio, ServicioEntradaResponseDto>();

            // Tarifas
            CreateMap<TarifaCrearRequestDto, Tarifa>();
            CreateMap<Tarifa, TarifaResponseDto>();

            // Trabajadores
            CreateMap<TrabajadorCrearRequestDto, Trabajador>();
            CreateMap<Trabajador, TrabajadorResponseDto>();

            // Usuarios
            CreateMap<Usuario, UsuarioDto>();

            // Vehiculos
            CreateMap<VehiculoCrearRequestDto, Vehiculo>();
            CreateMap<Vehiculo, VehiculoFullResponseDto>();
            CreateMap<Vehiculo, VehiculoDetailResponseDto>();
            CreateMap<Vehiculo, VehiculoSimplificadoResponseDto>();
        }
    }
}
