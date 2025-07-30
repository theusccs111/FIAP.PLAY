using FIAP.PLAY.Application.Biblioteca.Interfaces;
using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using FIAP.PLAY.Application.Biblioteca.Services;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Interfaces.Repository;
using FIAP.PLAY.Domain.Biblioteca.Jogos.Entities;
using FIAP.PLAY.Domain.Biblioteca.Jogos.Enums;
using FluentValidation;
using Moq;

namespace FIAP.PLAY.Tests.Application.Biblioteca.Services
{
    public class JogoServiceTests
    {
        private readonly IJogoService _jogoService;
        private readonly Mock<IUnityOfWork> _mockForUOF = new Mock<IUnityOfWork>();
        private readonly Mock<IRepository<Jogo>> _mockForRepository = new Mock<IRepository<Jogo>>();
        private readonly Mock<IValidator<JogoRequest>> _mockForValidator = new Mock<IValidator<JogoRequest>>();
        private readonly Mock<ILoggerManager<JogoService>> _mockLogger = new Mock<ILoggerManager<JogoService>>();

        public JogoServiceTests()
        {
            _mockForUOF.Setup(d => d.Jogos).Returns(_mockForRepository.Object);
            _jogoService = new JogoService(_mockForUOF.Object, _mockForValidator.Object, _mockLogger.Object);
        }

        [Fact]
        public void ObterJogos_Valido_DeveRetornarTodosJogos()
        {
            // Prepare
            var jogo = Jogo.Criar("Super mario", 100, EGenero.Ação, 1993, "Nintendo");
            var jogos = new List<Jogo>() { jogo };

            _mockForRepository.Setup(d => d.GetAll()).Returns(jogos);

            // Act
            var resultado = _jogoService.ObterJogos();

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Single(resultado.Data!);
        }

        [Fact]
        public void ObterJogoPorId_Valido_DeveRetornarOJogoPeloId()
        {
            // Prepare
            long id = 1;
            var jogo = Jogo.Criar("Super mario", 100, EGenero.Ação, 1993, "Nintendo");
            jogo.Id = id;

            _mockForRepository.Setup(d => d.GetById(It.IsAny<long>())).Returns(jogo);

            // Act
            var resultado = _jogoService.ObterJogoPorId(id);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Success);
            Assert.Equal(id, resultado.Data!.Id);
            Assert.Equal(jogo.Preco, resultado.Data!.Preco);
            Assert.Equal(jogo.Titulo, resultado.Data!.Titulo);
            Assert.Equal(jogo.Genero, resultado.Data!.Genero);
            Assert.Equal(jogo.AnoLancamento, resultado.Data!.AnoLancamento);
            Assert.Equal(jogo.Desenvolvedora, resultado.Data!.Desenvolvedora);
        }
    }
}
