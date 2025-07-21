using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Application.UserAccess.Resource.Response
{
    public class LoginResponse
    {
        public LoginResponse()
        {
        }

        public long UserId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public bool EstaAutenticado { get; set; }
    }
}
