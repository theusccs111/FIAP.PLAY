using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Infrastructure.Logging.Correlation
{
    public interface ICorrelationIdGenerator
    {
        string Get();
        void Set(string correlationId);
    }
}
