using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.UserAccess.Resource.Request;
using FIAP.PLAY.Application.UserAccess.Services;
using FIAP.PLAY.Domain.UserAccess.Entities;
using FIAP.PLAY.Domain.UserAccess.Enums;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;

namespace FIAP.PLAY.Tests.Application.UserAccess.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUnityOfWork> _uowMock = new();
        private readonly Mock<IValidator<UserRequest>> _validatorMock = new();
        private readonly Mock<ILoggerManager<UserService>> _loggerMock = new();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();

        private readonly UserService _service;

        public UserServiceTests()
        {
            _service = new UserService(
                _httpContextAccessorMock.Object,
                _uowMock.Object,
                _validatorMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnUserResponse_WhenRequestIsValid()
        {
            // Arrange
            var request = new UserRequest("Jacqueline", "Has@!12356", "jaque@email.com", ERole.Admin, true);

            _validatorMock.Setup(v => v.Validate(request)).Returns(new ValidationResult());

            var userEntity = User.Criar(request.Name, request.PasswordHash, request.Email, request.Role, request.Active);

            _uowMock.Setup(u => u.Users.CreateAsync(It.IsAny<User>()))
                    .ReturnsAsync((User u) => { u.Id = 1; return u; });

            _uowMock.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateUserAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.Id);
            Assert.Equal("Jacqueline", result.Data.Name);

            _uowMock.Verify(u => u.Users.CreateAsync(It.IsAny<User>()), Times.Once);
            _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
            _loggerMock.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Once);
        }
    }
}