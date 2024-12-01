using System.Text.Json.Serialization;

namespace ViteMontevideo_API.Services.Dtos.Common
{
    public class ApiResponse
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? Data { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? ValidationErrors { get; set; }

        public static ApiResponse Success(string message) => new()
        {
            Title = "Éxito",
            Message = message,
        };

        public static ApiResponse Success(string message, object data) => new()
        {
            Title = "Éxito",
            Data = data,
            Message = message,
        };

        public static ApiResponse UnProcessableEntity(List<string> errors) => new()
        {
            Title = "Error de validación",
            Message = "Uno o más errores de validación ocurrieron.",
            ValidationErrors = errors.Any() ? errors : null,
        };
    }
}
