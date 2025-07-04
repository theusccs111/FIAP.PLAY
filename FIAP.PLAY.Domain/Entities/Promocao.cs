using FIAP.PLAY.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Domain.Entities
{
    public class Promocao : EntidadeBase
    {
        public long UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
        public decimal DescontoPercentual { get; set; }
        public bool Ativo { get; set; }
    }
}
