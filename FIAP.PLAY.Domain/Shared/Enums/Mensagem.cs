using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FIAP.PLAY.Domain.Shared.Enums
{
    public enum Mensagem
    {
        [Description("Ocorreu um erro no sistema")]
        ErrorSystem = 0,
        [Description("Nada encontrado")]
        NotFound = 1,
        [Description("Item Criado")]
        ItemCreated = 2,
        [Description("Item Alterado")]
        ItemUpdated = 2,
        [Description("Item Deletado")]
        ItemDeleted = 3,
        [Description("O \"{0}\" deve ser informado.")]
        FieldRequired = 4
    }
}
