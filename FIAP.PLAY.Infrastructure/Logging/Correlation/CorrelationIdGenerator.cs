using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Infrastructure.Logging.Correlation
{
    public class CorrelationIdGenerator : ICorrelationIdGenerator
    {
        private static string _correlationId;

        public string Get() => _correlationId;

        public void Set(string correlationId) => _correlationId = correlationId;
    }
}
