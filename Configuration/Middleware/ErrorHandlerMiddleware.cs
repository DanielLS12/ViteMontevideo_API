﻿using System.Net;
using ViteMontevideo_API.Exceptions;
using ViteMontevideo_API.Presentation.Dtos.Common;

namespace ViteMontevideo_API.Configuration.Middleware
{
    public class ErrorHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger) =>
            _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (TaskCanceledException ex) when (context.RequestAborted.IsCancellationRequested)
            {
                // No loggear excepciones causadas por la cancelación de la solicitud del cliente
                _logger.LogError(ex.Message);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/problem+json";

            var statusCode = ex switch
            {
                BadRequestException => HttpStatusCode.BadRequest,
                NotFoundException => HttpStatusCode.NotFound,
                NotImplementedException => HttpStatusCode.NotImplemented,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                ForbiddenException => HttpStatusCode.Forbidden,
                ConflictException => HttpStatusCode.Conflict,
                KeyNotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError,
            };

            response.StatusCode = (int)statusCode;

            // Solo registrar si es un error inesperado (5xx)
            if (response.StatusCode >= 500 && response.StatusCode <= 599)
                _logger.LogError(ex, $"Ha ocurrido un problema inesperado. {ex.Message}");

            var apiResponse = new ApiResponse
            {
                Title = GetTitleForStatusCode(statusCode),
                Message = ex.Message,
            };

            return response.WriteAsJsonAsync(apiResponse);
        }

        private static string GetTitleForStatusCode(HttpStatusCode statusCode) =>
            statusCode switch
            {
                HttpStatusCode.BadRequest => "Petición inválida",
                HttpStatusCode.NotFound => "Recurso no encontrado",
                HttpStatusCode.NotImplemented => "Característica no implementada",
                HttpStatusCode.Forbidden => "Acceso prohibido",
                HttpStatusCode.Unauthorized => "Acceso no autorizado",
                HttpStatusCode.Conflict => "Conflicto",
                _ => "Error en el servidor",
            };
    }
}
