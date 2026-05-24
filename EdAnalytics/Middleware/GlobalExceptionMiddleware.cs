using System.Net;
using System.Text.Json;

namespace EdAnalytics.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exceção não tratada capturada pelo middleware global.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, title) = exception switch
            {
                KeyNotFoundException => ((int)HttpStatusCode.NotFound, "Recurso não encontrado"),
                ArgumentException => ((int)HttpStatusCode.BadRequest, "Requisição inválida"),
                UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "Não autorizado"),
                _ => ((int)HttpStatusCode.InternalServerError, "Erro interno do servidor")
            };

            context.Response.StatusCode = statusCode;

            var response = new
            {
                type = "https://tools.ietf.org/html/rfc7807",
                title,
                status = statusCode,
                detail = exception.Message,
                traceId = context.TraceIdentifier
            };

            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(response, jsonOptions);
            await context.Response.Body.WriteAsync(jsonBytes, 0, jsonBytes.Length);
        }
    }
}
