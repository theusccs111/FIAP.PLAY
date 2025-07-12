using FIAP.PLAY.Infrastructure.Logging.Correlation;
using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace FIAP.PLAY.Web.Middleware
{
        public class CorrelationMiddleware
        {
            private readonly RequestDelegate _next;
            private const string _correlationIdHeader = "x-correlation-id";

            public CorrelationMiddleware(RequestDelegate next) => _next = next;


            public async Task Invoke(HttpContext context, ICorrelationIdGenerator correlationIdGenerator)
            {
                var correlationId = GetCorrelationId(context, correlationIdGenerator);
                AddCorrelationIdHeaderToResponse(context, correlationId);

                // Adiciona a propriedade estruturada no contexto Serilog
                using (LogContext.PushProperty("CorrelationId", correlationId.ToString()))
                {
                    await _next(context);
                }
        }

            private static StringValues GetCorrelationId(HttpContext context, ICorrelationIdGenerator correlationIdGenerator)
            {
                if (context.Request.Headers.TryGetValue(_correlationIdHeader, out var correlationId))
                {
                    correlationIdGenerator.Set(correlationId);
                    return correlationId;
                }
                else
                {
                    correlationId = Guid.NewGuid().ToString();
                    correlationIdGenerator.Set(correlationId);
                    return correlationId;
                }
            }

            private static void AddCorrelationIdHeaderToResponse(HttpContext context, StringValues correlationId)
           => context.Response.OnStarting(() =>
           {
               context.Response.Headers[_correlationIdHeader] = new[] { correlationId.ToString() };
               return Task.CompletedTask;
           });
        }
}
