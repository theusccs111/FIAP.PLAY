using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Application.Shared.Interfaces.Infrastructure
{
    public interface ILoggerManager<T>
    {
        void LogInformation(string message);
        void LogError(string message);
        void LogWarning(string message);
    }

}
