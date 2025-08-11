using FIAP.PLAY.Application.Library.Interfaces;
using FIAP.PLAY.Application.Library.Resource.Request;
using FIAP.PLAY.Application.Library.Services;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Interfaces.Repository;
using FIAP.PLAY.Domain.Library.Entities;
using FIAP.PLAY.Domain.Library.Enums;
using FIAP.PLAY.Domain.Shared.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace FIAP.PLAY.Tests.Application.Biblioteca.Services
{
    public class GameLibraryServiceTests
    {
        private readonly Mock<IUnityOfWork> _uowMock = new();
        private readonly Mock<IValidator<GameLibraryRequest>> _validatorMock = new();
        private readonly Mock<ILoggerManager<GameLibraryRequest>> _loggerMock = new();
        private readonly Mock<IRepository<GameLibrary>> _gameLibraryRepoMock = new();
        private readonly IGameLibraryService _service;

        public GameLibraryServiceTests()
        {            
            _service = new GameLibraryService(
                _uowMock.Object,
                _validatorMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task AddGameToLibraryAsync_LibraryNotFound_ThrowsNotFoundException()
        {
            long anyLibraryId = 1, anyGameId = 2;

            _uowMock.Setup(u => u.Libraries.GetByIdAsync(anyLibraryId))!
                    .ReturnsAsync((Library?)null);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.AddGameToLibraryAsync(anyLibraryId, anyGameId, CancellationToken.None)
            );
        }

        [Fact]
        public async Task AddGameToLibraryAsync_GameNotFound_ThrowsNotFoundException()
        {
            var library = Library.Create(42);
            
            _uowMock.Setup(u => u.Libraries.GetByIdAsync(It.IsAny<long>()))
                    .ReturnsAsync(library);

            _uowMock.Setup(u => u.Games.GetByIdAsync(It.IsAny<long>()))!
                    .ReturnsAsync((Game?)null);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.AddGameToLibraryAsync(1, 99, CancellationToken.None)
            );
        }

        [Fact]
        public async Task AddGameToLibraryAsync_InvalidRequest_ThrowsValidationException()
        {
            var library = Library.Create(1);
            var game = Game.Create("xyz", 10m, PLAY.Domain.Library.Enums.EGenre.Terror, 2020, "dev");
            
            _uowMock.Setup(u => u.Libraries.GetByIdAsync(It.IsAny<long>()))
                    .ReturnsAsync(library);
           
            _uowMock.Setup(u => u.Games.GetByIdAsync(It.IsAny<long>()))
                    .ReturnsAsync(game);

            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Library", "Biblioteca é obrigatória.")
            };
            _validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<GameLibraryRequest>(), CancellationToken.None))
                .ReturnsAsync(new ValidationResult(failures));

            await Assert.ThrowsAsync<PLAY.Domain.Shared.Exceptions.ValidationException>(
                () => _service.AddGameToLibraryAsync(1, 1, CancellationToken.None)
            );
        }

        [Fact]
        public async Task AddGameToLibraryAsync_ValidRequest_ReturnsGameLibraryResponse()
        {
            var library = Library.Create(100);
            library.Id = 1;

            var game = Game.Create(
                "Super Game", 49.90m, EGenre.FPS, 2021, "StudioX"
            );
            game.Id = 2;

            _uowMock.Setup(u => u.Libraries.GetByIdAsync(library.Id))
                    .ReturnsAsync(library);

            _uowMock.Setup(u => u.Games.GetByIdAsync(game.Id))
                    .ReturnsAsync(game);

            _validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<GameLibraryRequest>(), CancellationToken.None))
                .ReturnsAsync(new ValidationResult());

            _gameLibraryRepoMock.Setup(u => u.CreateAsync(It.IsAny<GameLibrary>()))
                .ReturnsAsync((GameLibrary g) => g);

            _uowMock.Setup(u => u.GameLibraries).Returns(_gameLibraryRepoMock.Object);
            _uowMock.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

            var result = await _service.AddGameToLibraryAsync(library.Id, game.Id, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(library.Id, result.Data!.LibraryId);
            Assert.Equal(game.Id, result.Data.Game.Id);
            Assert.Equal(game.Price, result.Data.PricePaid);

            _gameLibraryRepoMock.Verify(r => r.CreateAsync(It.IsAny<GameLibrary>()), Times.Once);
            _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
            _loggerMock.Verify(l => l.LogInformation(
                It.Is<string>(s => s.Contains($"Jogo {game.Title} adicionado"))),
                Times.Once
            );
        }

        [Fact]
        public async Task RemoveGameLibraryAsync_GameLibraryNotFound_ThrowsNotFoundException()
        {
            long anyGameLibraryId = 1;
            _uowMock.Setup(u => u.GameLibraries.GetByIdAsync(anyGameLibraryId))!
                    .ReturnsAsync((GameLibrary?)null);
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.RemoveGameLibraryAsync(anyGameLibraryId, CancellationToken.None)
            );
        }

        [Fact]
        public async Task RemoveGameLibraryAsync_ValidId_RemovesGameLibrary()
        {
            var gameLibrary = GameLibrary.Create(
                Library.Create(1),
                Game.Create("Game", 20m, EGenre.Aventura, 2022, "Dev")
            );

            gameLibrary.Id = 1;
            _uowMock.Setup(u => u.GameLibraries.GetByIdAsync(gameLibrary.Id))
                    .ReturnsAsync(gameLibrary);

            _uowMock.Setup(u => u.GameLibraries.DeleteAsync(gameLibrary.Id))
                    .Returns(Task.CompletedTask);

            _uowMock.Setup(u => u.CompleteAsync())
                    .Returns(Task.CompletedTask);

            await _service.RemoveGameLibraryAsync(gameLibrary.Id, CancellationToken.None);

            _uowMock.Verify(u => u.GameLibraries.DeleteAsync(gameLibrary.Id), Times.Once);

            _uowMock.Verify(u => u.CompleteAsync(), Times.Once);

            _loggerMock.Verify(l => l.LogInformation(
                It.Is<string>(s => s.Contains($"Registro de jogo com ID {gameLibrary.Id} removido"))),
                Times.Once);
        }

        [Fact]
        public async Task GetGamesByLibraryIdAsync_LibraryNotFound_ThrowsNotFoundException()
        {
            long anyLibraryId = 1;
            _uowMock.Setup(u => u.Libraries.GetByIdAsync(anyLibraryId))!
                    .ReturnsAsync((Library?)null);
            await Assert.ThrowsAsync<NotFoundException>(
                () => _service.GetGamesByLibraryIdAsync(anyLibraryId, CancellationToken.None)
            );
        }
    }
}
