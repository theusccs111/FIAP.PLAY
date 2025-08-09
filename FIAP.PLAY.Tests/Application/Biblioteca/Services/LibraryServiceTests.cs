using FIAP.PLAY.Application.Library.Interfaces;
using FIAP.PLAY.Application.Library.Resource.Request;
using FIAP.PLAY.Application.Library.Services;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Interfaces.Repository;
using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Application.UserAccess.Interfaces;
using FIAP.PLAY.Application.UserAccess.Resource.Response;
using FIAP.PLAY.Domain.Library.Entities;
using FIAP.PLAY.Domain.Shared.Exceptions;
using FIAP.PLAY.Domain.UserAccess.Enums;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Linq.Expressions;

namespace FIAP.PLAY.Tests.Application.Biblioteca.Services
{
    public class LibraryServiceTests
    {
        private readonly ILibraryService _service;
        private readonly Mock<IUnityOfWork> _uowMock = new();
        private readonly Mock<IRepository<Library>> _repoMock = new();
        private readonly Mock<IValidator<LibraryRequest>> _validatorMock = new();
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly Mock<ILoggerManager<LibraryRequest>> _loggerMock = new();

        public LibraryServiceTests()
        {
            // Validação sempre bem-sucedida
            _validatorMock
                .Setup(v => v.Validate(It.IsAny<LibraryRequest>()))
                .Returns(new ValidationResult());

            _uowMock.Setup(u => u.Libraries).Returns(_repoMock.Object);

            _service = new LibraryService(
                _uowMock.Object,
                _validatorMock.Object,
                _userServiceMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task CreateLibraryAsync_ComUsuarioExistente_CriaBiblioteca()
        {
            // Arrange
            var request = new LibraryRequest(42);

            // Mock do IUserService
            _userServiceMock
                .Setup(u => u.GetUserByIdAsync(42, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<UserResponse>(
                    new UserResponse(42, "Alice", "alice@fiap.com.br", ERole.Common)
                ));

            // Simula que não há biblioteca existente
            _repoMock
                .Setup(r => r.GetFirstAsync(It.IsAny<Expression<Func<Library, bool>>>()))!
                .ReturnsAsync((Library?)null);

            // Simula inserção (retorna a entidade criada com Id)
            _repoMock
                .Setup(r => r.CreateAsync(It.IsAny<Library>()))
                .ReturnsAsync((Library lib) =>
                {                    
                    lib.Id = 100;
                    return lib;
                });

            // Act
            var result = await _service.CreateLibraryAsync(request, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(100, result.Data!.Id);
            Assert.Equal(42, result.Data.UserId);

            // Verifica que chamou CreateAsync uma vez
            _repoMock.Verify(r => r.CreateAsync(
                It.Is<Library>(l => l.UserId == 42)
            ), Times.Once);

            // Verifica que chamou SaveChangesAsync no UoW
            _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateLibraryAsync_ComUsuarioInexistente_LancaNotFoundException()
        {
            // Arrange
            var request = new LibraryRequest(77);

            // Simula usuário não encontrado
            _userServiceMock
                .Setup(u => u.GetUserByIdAsync(77, It.IsAny<CancellationToken>()))!
                .ReturnsAsync((Result<UserResponse>?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.CreateLibraryAsync(request, CancellationToken.None)
            );
        }

        [Fact]
        public async Task CreateLibraryAsync_ComBibliotecaExistente_LancaValidationException()
        {
            // Arrange
            var request = new LibraryRequest(42);            

            // Mock do IUserService
            _userServiceMock
                .Setup(u => u.GetUserByIdAsync(42, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<UserResponse>(
                    new UserResponse(42, "Alice", "alice@fiap.com.br", ERole.Common)
                ));

            // Simula que já existe uma biblioteca para o usuário
            _repoMock
                .Setup(r => r.GetFirstAsync(It.IsAny<Expression<Func<Library, bool>>>()))!
                .ReturnsAsync(Library.Create(42));

            // Act & Assert
            await Assert.ThrowsAsync<PLAY.Domain.Shared.Exceptions.ValidationException>(() =>
                _service.CreateLibraryAsync(request, CancellationToken.None)
            );
        }

        [Fact]
        public async Task DeleteLibraryAsync_ComBibliotecaExistente_DeletaBiblioteca()
        {
            // Arrange
            var libraryId = 100;

            // Simula que a biblioteca existe
            _repoMock
                .Setup(r => r.GetByIdAsync(libraryId))
                .ReturnsAsync(Library.Create(42));

            // Act
            await _service.DeleteLibraryAsync(libraryId, CancellationToken.None);

            // Assert
            // Verifica que chamou DeleteAsync uma vez
            _repoMock.Verify(r => r.DeleteAsync(libraryId), Times.Once);

            // Verifica que chamou SaveChangesAsync no UoW
            _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteLibraryAsync_ComBibliotecaInexistente_LancaNotFoundException()
        {
            // Arrange
            var libraryId = 999;

            // Simula que a biblioteca não existe
            _repoMock
                .Setup(r => r.GetByIdAsync(libraryId))!
                .ReturnsAsync((Library?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                _service.DeleteLibraryAsync(libraryId, CancellationToken.None)
            );
        }
    }
}
