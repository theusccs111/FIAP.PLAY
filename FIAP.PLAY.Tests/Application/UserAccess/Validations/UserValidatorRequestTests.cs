using FIAP.PLAY.Application.UserAccess.Resource.Request;
using FIAP.PLAY.Application.UserAccess.Validations;
using FluentValidation.TestHelper;

namespace FIAP.PLAY.Tests.Application.UserAccess.Validations
{
    public class UserValidatorRequestTests
    {
        private readonly UserRequestValidator _validator;

        public UserValidatorRequestTests()
        {
            _validator = new UserRequestValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Nome_Is_Empty()
        {
            var model = new UserRequest { Name = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(user => user.Name);
        }

        [Theory]
        [InlineData("Jo")]
        [InlineData("A")]
        public void Should_Have_Error_When_Nome_Is_Too_Short(string nome)
        {
            var model = new UserRequest { Name = nome };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(user => user.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            var model = new UserRequest { Email = "email-invalido" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(user => user.Email);
        }

        [Fact]
        public void Should_Have_Error_When_Senha_Is_Empty()
        {
            var model = new UserRequest { PasswordHash = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(user => user.PasswordHash);
        }

        [Theory]
        [InlineData("abc123")] // muito curta
        [InlineData("abcdefghijklmnopq")] // muito longa
        [InlineData("abcdefgh")] // sem número, sem maiúscula, sem especial
        [InlineData("ABCDEFGH1")] // sem minúscula e sem especial
        [InlineData("abcdefgh1")] // sem maiúscula e sem especial
        [InlineData("ABCdefgh")] // sem número e sem especial
        [InlineData("ABCdefgh1")] // sem especial
        public void Should_Have_Error_When_Senha_Invalid(string senha)
        {
            var model = new UserRequest { PasswordHash = senha };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(user => user.PasswordHash);
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
        {
            var model = new UserRequest
            {
                Name = "João da Silva",
                Email = "joao@email.com",
                PasswordHash = "Senha123!"
            };

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
