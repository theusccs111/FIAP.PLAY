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

        public long UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public TipoPerfil Perfil { get; set; }
        public string PerfilDescricao { get { return Perfil.GetDescription(); } }
        public string Token { get; set; }
        public bool EstaAutenticado { get; set; }
    }
}
