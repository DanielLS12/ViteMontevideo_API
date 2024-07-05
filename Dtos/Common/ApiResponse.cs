using Microsoft.AspNetCore.Mvc;

namespace ViteMontevideo_API.Dtos.Common
{
    public class ApiResponse
    {
        public int Status { get; set; }
        public bool IsSuccess { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public List<string> ValidationErrors { get; set; } = new List<string>();

        public static ApiResponse Success(string message) => new()
        {
            Status = 200,
            IsSuccess = true,
            Title = "Éxito",
            Message = message,
        };

        public static ApiResponse SuccessCreated(string message) => new()
        {
            Status = 201,
            IsSuccess = true,
            Title = "Éxito",
            Message = message,
        };

        public static ApiResponse UnProcessableEntity(List<string> errors) => new()
        {
            Status = 422,
            IsSuccess = false,
            Title = "Error de validación",
            Message = "Uno o más errores de validación ocurrieron.",
            ValidationErrors = errors,
        };
    }
}
