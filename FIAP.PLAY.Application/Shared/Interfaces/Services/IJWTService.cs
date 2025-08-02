using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Application.Shared.Interfaces.Services
{
    public interface IJWTService
    {
        string GenerateToken(string userId, string userName, string userEmail, string role);
    }
}
