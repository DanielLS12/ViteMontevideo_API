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
                    Title = "ViteMontevideo",
                    Version = "v1",
                    Description = "API para ViteMontevideo (.NET MAUI)"
                });

                c.SchemaFilter<EnumSchemaFilter>();
            });
        }
    }
}
