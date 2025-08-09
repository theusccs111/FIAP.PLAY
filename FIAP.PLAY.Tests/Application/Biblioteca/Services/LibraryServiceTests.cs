using FIAP.PLAY.Application.Library.Interfaces;
using FIAP.PLAY.Application.Library.Resource.Request;
using FIAP.PLAY.Application.Library.Services;
using FIAP.PLAY.Application.Shared.Interfaces;
using FIAP.PLAY.Application.Shared.Interfaces.Infrastructure;
using FIAP.PLAY.Application.Shared.Interfaces.Repository;
using FIAP.PLAY.Application.Shared.Services;
using FIAP.PLAY.Application.UserAccess.Interfaces;
using FIAP.PLAY.Application.UserAccess.Resource.Response;
using FIAP.PLAY.Domain.Library.Entities;
using FIAP.PLAY.Domain.UserAccess.Enums;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Linq.Expressions;

namespace FIAP.PLAY.Tests.Application.Biblioteca.Services
{
    public class LibraryServiceTests
    {
        private readonly Mock<IUnityOfWork> _uowMock = new();
        private readonly Mock<IRepository<Library>> _repoMock = new();
        private readonly Mock<IValidator<LibraryRequest>> _validatorMock = new();
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly Mock<ILoggerManager<LibraryRequest>> _loggerMock = new();
        private readonly ILibraryService _service;

        public LibraryServiceTests()
        {         

            // configuração padrão: validador sem erros
            _validatorMock
                .Setup(v => v.Validate(It.IsAny<LibraryRequest>()))
                .Returns(new ValidationResult());

            // uow.Libraries retorna nosso fake repository
            _uowMock
                .SetupGet(u => u.Libraries)
                .Returns(_repoMock.Object);

            _service = new LibraryService(
                _uowMock.Object,
                _validatorMock.Object,
                _userServiceMock.Object,
                _loggerMock.Object
            );
        }

    }

}
