using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ViteMontevideo_API.ActionFilters
{
    // Cambio de los valores subyancentes de los enums por sus nombres. 
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Enum.Clear();
                var enumValues = Enum.GetValues(context.Type).Cast<int>();

                foreach (var value in enumValues)
                {
                    var name = Enum.GetName(context.Type, value);
                    schema.Enum.Add(new OpenApiString($"{name}"));
                }
            }
        }
    }
}
