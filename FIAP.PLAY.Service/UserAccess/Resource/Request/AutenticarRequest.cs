using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Domain.Shared.Resource.Request
{
    public class AutenticarRequest
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
