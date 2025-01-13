using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Presentation.ActionFilters;
using ViteMontevideo_API.Presentation.Middleware;
using ViteMontevideo_API.Services.Implementation;
using ViteMontevideo_API.Persistence.Repositories.Implementation;

namespace ViteMontevideo_API.Configuration
{
    public static class ServiceConfiguration
    {
        public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            var secretKey = configuration.GetSection("JwtSettings").GetSection("SecretKey").Value;
            var bytesKey = Encoding.UTF8.GetBytes(secretKey!);

            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(bytesKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            services.AddDbContext<EstacionamientoContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("SqlString")));

            services.AddAutoMapper(typeof(Program).Assembly);

            services.AddCors(opt =>
            {
                opt.AddPolicy("ReglasCors", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            // Repositories
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IActividadRepository, ActividadRepository>();
            services.AddScoped<ICargoRepository, CargoRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IVehiculoRepository, VehiculoRepository>();
            services.AddScoped<ITarifaRepository, TarifaRepository>();
            services.AddScoped<IEgresoRepository, EgresoRepository>();
            services.AddScoped<ICajaChicaRepository, CajaChicaRepository>();
            services.AddScoped<ITrabajadorRepository, TrabajadorRepository>();
            services.AddScoped<IContratoAbonadoRepository, ContratoAbonadoRepository>();
            services.AddScoped<IServicioRepository, ServicioRepository>();
            services.AddScoped<IComercioAdicionalRepository, ComercioAdicionalRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            // Services
            services.AddScoped<IActividadService, ActividadService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<ICargoService, CargoService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IVehiculoService, VehiculoService>();
            services.AddScoped<ITarifaService, TarifaService>();
            services.AddScoped<IEgresoService, EgresoService>();
            services.AddScoped<ITrabajadorService, TrabajadorService>();
            services.AddScoped<IContratoAbonadoService, ContratoAbonadoService>();
            services.AddScoped<IComercioAdicionalService, ComercioAdicionalService>();
            services.AddScoped<IServicioService, ServicioService>();
            services.AddScoped<ICajaChicaService, CajaChicaService>();
            services.AddScoped<IUsuarioService, UsuarioService>();

            services.AddTransient<ErrorHandlerMiddleware>();
            services.AddScoped<ValidationFilterAttribute>();

            services.Configure<ApiBehaviorOptions>(opt =>
                opt.SuppressModelStateInvalidFilter = true);
        }
    }
}
