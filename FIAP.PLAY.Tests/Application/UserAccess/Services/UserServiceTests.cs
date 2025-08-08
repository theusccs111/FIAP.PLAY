using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.UserAccess.Resource.Request;
using FIAP.PLAY.Application.UserAccess.Services;
using FIAP.PLAY.Domain.UserAccess.Entities;
using FIAP.PLAY.Domain.UserAccess.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Moq;

namespace FIAP.PLAY.Tests.Application.UserAccess
{
    public class UserServiceTests
    {
        private readonly Mock<IUnityOfWork> _uowMock;
        private readonly Mock<IValidator<UserRequest>> _validatorMock;
        private readonly Mock<ILoggerManager<UserService>> _loggerMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _uowMock = new Mock<IUnityOfWork>();
            _validatorMock = new Mock<IValidator<UserRequest>>();
            _loggerMock = new Mock<ILoggerManager<UserService>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _service = new UserService(
                _httpContextAccessorMock.Object,
                _uowMock.Object,
                _validatorMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GetUsersAsync_ShouldReturnUsers()
        {
            // Arrange
            var users = new List<User>
            {
                User.Criar("User1", "hash", "user1@email.com",ERole.Common, true),
                User.Criar("User2", "hash", "user2@email.com",ERole.Common, true)
            };
            _uowMock.Setup(u => u.Users.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _service.GetUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count());
            Assert.Equal("User1", result.Data.First().Name);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldThrow_WhenValidationFails()
        {
            // Arrange
            var request = new UserRequest(); // inválido
            var validationFailures = new List<FluentValidation.Results.ValidationFailure>
            {
                new("Email", "Email é obrigatório")
            };
            var validationResult = new FluentValidation.Results.ValidationResult(validationFailures);
            _validatorMock.Setup(v => v.Validate(request)).Returns(validationResult);

            // Act & Assert
            await Assert.ThrowsAsync<PLAY.Domain.Shared.Exceptions.ValidationException>(() =>
                _service.CreateUserAsync(request));
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnCreatedUser_WhenValid()
        {
            // Arrange
            var request = new UserRequest
            {
                Name = "Test",
                Email = "test@email.com",
                PasswordHash = "abc123",
                Role = ERole.Common,
                Active = true
            };

            var createdUser = User.Criar(request.Name, request.PasswordHash, request.Email, request.Role, request.Active);
            typeof(User).GetProperty("Id")!.SetValue(createdUser, 1L); // set ID manually if needed

            _validatorMock.Setup(v => v.Validate(request))
                         .Returns(new FluentValidation.Results.ValidationResult());

            _uowMock.Setup(u => u.Users.CreateAsync(It.IsAny<User>()))
                    .ReturnsAsync(createdUser);

            _uowMock.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateUserAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal("Test", result.Data.Name);
            Assert.Equal("test@email.com", result.Data.Email);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldThrow_WhenUserNotExists()
        {
            // Arrange
            _uowMock.Setup(u => u.Users.ExistsAsync(99)).ReturnsAsync(false);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<PLAY.Domain.Shared.Exceptions.NotFoundException>(
                () => _service.DeleteUserAsync(99));

            Assert.Contains("Usuário não encontrado", ex.Message);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldThrow_WhenIdIsZero()
        {
            // Arrange
            var request = new UserRequest();

            // Act & Assert
            var ex = await Assert.ThrowsAsync<PLAY.Domain.Shared.Exceptions.ValidationException>(
                () => _service.UpdateUserAsync(0, request));

            Assert.Contains("id do usuário não pode ser nulo", ex.Message);
        }
    }
}
