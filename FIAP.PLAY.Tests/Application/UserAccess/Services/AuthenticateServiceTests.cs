using FIAP.PLAY.Application.Biblioteca.Interfaces;
using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using FIAP.PLAY.Application.Biblioteca.Services;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Interfaces.Repository;
using FIAP.PLAY.Application.Shared.Interfaces.Services;
using FIAP.PLAY.Application.UserAccess.Interfaces;
using FIAP.PLAY.Application.UserAccess.Resource.Request;
using FIAP.PLAY.Application.UserAccess.Services;
using FIAP.PLAY.Domain.Biblioteca.Jogos.Entities;
using FIAP.PLAY.Domain.Shared.Extensions;
using FIAP.PLAY.Domain.UserAccess.Entities;
using FIAP.PLAY.Domain.UserAccess.Enums;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Tests.Application.UserAccess.Services
{
    public class AuthenticateServiceTests
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly Mock<IUnityOfWork> _mockUow = new();
        private readonly Mock<IValidator<AuthenticateRequest>> _mockValidator = new();
        private readonly Mock<ILoggerManager<AuthenticateService>> _mockLogger = new();
        private readonly Mock<IJWTService> _mockJwtService = new();
        private readonly Mock<IHttpContextAccessor> _mockHttpContext = new();
        private readonly Mock<IRepository<User>> _mockUserRepository = new();

        public AuthenticateServiceTests()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "usuario_teste"),
                new Claim(ClaimTypes.Email, "teste@fiap.com.br"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var mockHttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };

            _mockHttpContext.Setup(x => x.HttpContext).Returns(mockHttpContext);
            _mockUow.Setup(x => x.Users).Returns(_mockUserRepository.Object);

            _authenticateService = new AuthenticateService(
                _mockHttpContext.Object,
                _mockUow.Object,
                _mockValidator.Object,
                _mockLogger.Object,
                _mockJwtService.Object);
        }

        [Fact]
        public async Task LoginAsync_ComCredenciaisValidas_DeveRetornarToken()
        {
            var request = new AuthenticateRequest
            {
                Email = "teste@fiap.com.br",
                Password = "123456!Aa"
            };

            var usuario = new User
            {
                Id = 1,
                Name = "Usuario Teste",
                Email = request.Email,
                Role = FIAP.PLAY.Domain.UserAccess.Enums.ERole.Admin,
                PasswordHash = request.Password.Encrypt()
            };

            _mockValidator.Setup(v => v.Validate(It.IsAny<AuthenticateRequest>()))
                .Returns(new ValidationResult());

            _mockUserRepository.Setup(r => r.GetFirstAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(usuario);

            _mockJwtService.Setup(j => j.GenerateToken(
                usuario.Id.ToString(), usuario.Name, usuario.Email, usuario.Role.ToString()))
                .Returns("fake-jwt-token");

            var result = await _authenticateService.LoginAsync(request);

            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("fake-jwt-token", result.Data!.Token);
        }

        [Fact]
        public async Task LoginAsync_EmailInvalido_DeveRetornarErro()
        {
            var request = new AuthenticateRequest { Email = "naoexiste", Password = "123456" };

            var resultadoValidacaoInvalido = new ValidationResult([new ValidationFailure("Email", "Email informado deve ser válido")]);
            _mockValidator.Setup(v => v.Validate(It.IsAny<AuthenticateRequest>()))
                .Returns(resultadoValidacaoInvalido);

            await Assert.ThrowsAsync<FIAP.PLAY.Domain.Shared.Exceptions.ValidationException>(() => _authenticateService.LoginAsync(request));
        }

        [Fact]
        public async Task LoginAsync_UsuarioNaoEncontrado_DeveRetornarErro()
        {
            var request = new AuthenticateRequest { Email = "naoexiste@gmail.com", Password = "123456!1a" };
            var resultado = Assert.ThrowsAsync<FIAP.PLAY.Domain.Shared.Exceptions.NotFoundException>(() => _authenticateService.LoginAsync(request));

            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task LoginAsync_SenhaInvalida_DeveRetornarErro()
        {
            var request = new AuthenticateRequest
            {
                Email = "teste@fiap.com.br",
                Password = "invalida"
            };

            var usuario = new User
            {
                Id = 1,
                Name = "Usuario Teste",
                Email = request.Email,
                Role = ERole.Admin,
                PasswordHash = "Correta123!".Encrypt()
            };

            _mockValidator.Setup(v => v.Validate(It.IsAny<AuthenticateRequest>()))
                .Returns(new ValidationResult());

            _mockUserRepository.Setup(r => r.GetFirstAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(usuario);

            await Assert.ThrowsAsync<FIAP.PLAY.Domain.Shared.Exceptions.ValidationException>(() => _authenticateService.LoginAsync(request));


        }
    }
}
