using Microsoft.OpenApi.Models;
using ViteMontevideo_API.Presentation.ActionFilters;

namespace ViteMontevideo_API.Configuration
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Estacionamiento Montevideo",
                    Version = "v1",
                    Description = @"API para la gestión de un estacionamiento, 
                    diseñada para facilitar el control de vehículos, espacios disponibles y tarifas. 
                    Permite registrar ingresos y salidas de vehículos, calcular el costo por tiempo de permanencia,
                    y generar reportes de ocupación e ingresos.

                    Incluye autenticación por roles para garantizar un acceso seguro y controlado,
                    y permite la integración con sistemas externos para la sincronización de datos."
                });

                c.SchemaFilter<EnumSchemaFilter>();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Autorización",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Por favor, ingresar un token válido con el siguiente formato: {tu token aquí} no agregar la palabra 'Bearer' antes."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
        }
    }
}
