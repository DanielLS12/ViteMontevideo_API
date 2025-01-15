using AutoMapper;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Services.Dtos.Actividades.Requests;
using ViteMontevideo_API.Services.Dtos.Actividades.Responses;
using ViteMontevideo_API.Services.Dtos.CajasChicas.Requests;
using ViteMontevideo_API.Services.Dtos.CajasChicas.Responses;
using ViteMontevideo_API.Services.Dtos.Cargos.Requests;
using ViteMontevideo_API.Services.Dtos.Cargos.Responses;
using ViteMontevideo_API.Services.Dtos.Categorias.Requests;
using ViteMontevideo_API.Services.Dtos.Categorias.Responses;
using ViteMontevideo_API.Services.Dtos.Clientes.Requests;
using ViteMontevideo_API.Services.Dtos.Clientes.Responses;
using ViteMontevideo_API.Services.Dtos.ComerciosAdicionales.Requests;
using ViteMontevideo_API.Services.Dtos.ComerciosAdicionales.Responses;
using ViteMontevideo_API.Services.Dtos.ContratosAbonado.Requests;
using ViteMontevideo_API.Services.Dtos.ContratosAbonado.Responses;
using ViteMontevideo_API.Services.Dtos.Egresos.Requests;
using ViteMontevideo_API.Services.Dtos.Egresos.Responses;
using ViteMontevideo_API.Services.Dtos.Servicios.Requests;
using ViteMontevideo_API.Services.Dtos.Servicios.Responses;
using ViteMontevideo_API.Services.Dtos.Tarifas.Requests;
using ViteMontevideo_API.Services.Dtos.Tarifas.Responses;
using ViteMontevideo_API.Services.Dtos.Trabajadores.Requests;
using ViteMontevideo_API.Services.Dtos.Trabajadores.Responses;
using ViteMontevideo_API.Services.Dtos.Usuarios;
using ViteMontevideo_API.Services.Dtos.Vehiculos.Requests;
using ViteMontevideo_API.Services.Dtos.Vehiculos.Responses;

namespace ViteMontevideo_API.Services.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Actividades
            CreateMap<ActividadRequestDto, Actividad>();
            CreateMap<Actividad, ActividadResponseDto>();

            // Cajas Chicas
            CreateMap<CajaChicaCrearRequestDto, CajaChica>();
            CreateMap<CajaChica, CajaChicaResponseDto>();

            // Cargos
            CreateMap<CargoRequestDto, Cargo>();
            CreateMap<Cargo, CargoResponseDto>();

            // Categorias
            CreateMap<CategoriaRequestDto, Categoria>();
            CreateMap<Categoria, CategoriaResponseDto>();

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
            CreateMap<Usuario, UsuarioRequestDto>();
            CreateMap<Usuario, UsuarioResponseDto>();
            CreateMap<Usuario, UsuarioBasicResponseDto>();

            // Vehiculos
            CreateMap<VehiculoCrearRequestDto, Vehiculo>();
            CreateMap<Vehiculo, VehiculoFullResponseDto>();
            CreateMap<Vehiculo, VehiculoDetailResponseDto>();
            CreateMap<Vehiculo, VehiculoSimplificadoResponseDto>();
        }
    }
}
