using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Domain.Shared.Extensions;
using FIAP.PLAY.Domain.UserAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Application.UserAccess.Resource.Response
{
    public class LoginResponse
    {
        public  LoginResponse()
        {
        }

        public long UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ERole Role { get; set; }
        public string RoleDescription { get { return Role.GetDescription(); } }
        public string Token { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
