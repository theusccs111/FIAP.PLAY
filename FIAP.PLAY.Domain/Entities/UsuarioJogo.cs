using FIAP.PLAY.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Domain.Entities
{
    public class UsuarioJogo : EntidadeBase
    {
        public long UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
        public long JogoId { get; set; }
        public virtual Jogo Jogo { get; set; }
        public decimal ValorPago { get; set; }
    }
}
