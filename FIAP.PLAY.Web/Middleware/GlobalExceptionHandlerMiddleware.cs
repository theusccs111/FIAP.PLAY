using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Domain.Shared.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace FIAP.PLAY.Web.Middleware
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                ValidationException => (int)HttpStatusCode.BadRequest,
                NotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var resultado = new Resultado<object>
            {
                Success = false,
                Message = exception.Message,
                //MessageDetail = exception.InnerException?.Message,
                Data = null
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(resultado));
        }
    }
}
