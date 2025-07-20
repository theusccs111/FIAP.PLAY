using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Infrastructure.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Infrastructure.Logging
{
    public class LoggerManager<T> : ILoggerManager<T>
    {
        private readonly BaseLogger<T> _logger;

        public LoggerManager(BaseLogger<T> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogError(string message)
        {
            _logger.LogError(message);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }
    }
}
