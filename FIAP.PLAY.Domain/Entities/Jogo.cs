using FIAP.PLAY.Domain.Entities.Base;
using FIAP.PLAY.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Domain.Entities
{
    public class Jogo : EntidadeBase
    {
        public string Nome { get; set; }
        public TipoGenero Genero { get; set; }
        public decimal Valor { get; set; }
        public byte[] Imagem { get; set; }
        public string Desenvolvedora { get; set; }
        public DateTime DataLancamento { get; set; }
    }
}
