using FIAP.PLAY.Application.UserAccess.Resource.Request;
using FluentValidation;

namespace FIAP.PLAY.Application.UserAccess.Validations
{
    public class UserValidator : AbstractValidator<UsuarioRequest>
    {
        public UserValidator()
        {
            RuleFor(x => x.Nome)
           .NotEmpty().WithMessage(" O nome é obrigatório.")
           .Length(3, 100).WithMessage(" O nome deve ter entre 3 e 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(" O e-mail é obrigatório.")
                .EmailAddress().WithMessage("Informe um e-mail válido.");

            RuleFor(x => x.SenhaHash)
                .NotEmpty().WithMessage(" A senha é obrigatória.")
                .Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[!@#$%^&*(),.?"":{}|<>]).{8,}$")
                .WithMessage(" A senha deve ter no mínimo 8 caracteres e conter letras, números e caracteres especiais.");
        }
    }
}
