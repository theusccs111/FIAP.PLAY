using FIAP.PLAY.Infrastructure.Logging.Correlation;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Infrastructure.Logs
{
    public class BaseLogger<T>
    {
        protected readonly ILogger<T> _logger;
        protected readonly ICorrelationIdGenerator _correlationId;


        public BaseLogger(ILogger<T> logger, ICorrelationIdGenerator correlationId)
        {
            _logger = logger;
            _correlationId = correlationId;
        }

        public virtual void LogInformation(string message)
        {
            using (LogContext.PushProperty("CorrelationId", _correlationId.Get()))
            {
                _logger.LogInformation(message);
            }
        }

        public virtual void LogError(string message)
        {
            using (LogContext.PushProperty("CorrelationId", _correlationId.Get()))
            {
                _logger.LogError(message);
            }
        }

        public virtual void LogWarning(string message)
        {
            using (LogContext.PushProperty("CorrelationId", _correlationId.Get()))
            {
                _logger.LogWarning(message);
            }
        }
    }
}
