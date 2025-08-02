using System.ComponentModel;

namespace FIAP.PLAY.Domain.Biblioteca.Jogos.Enums
{
    public enum EGenre
    {
        [Description("Ação")]
        Acao = 1,
        Aventura = 2,
        MMORPG = 3,
        [Description("Estratégia")]
        Estrategia = 4,
        [Description("Simulação")]
        Simulacao = 5,
        Esporte = 6,
        Corrida = 7,
        Luta = 8,
        Terror = 9,
        Puzzle = 10,
        Tiro = 11,
        Plataforma = 12,
        FPS = 13,
        MOBA = 14,
        RPG = 15
    }
}
