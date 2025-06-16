using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Domain.Resource.Request
{
    public class AutenticarRequest
    {
        public string Login { get; set; }
        public string Senha { get; set; }
    }
}
