using FIAP.PLAY.Domain.Shared.Enums;
using FIAP.PLAY.Domain.Shared.Extensions;
using FIAP.PLAY.Domain.UserAccess.Entities;
using FluentValidation;

namespace FIAP.PLAY.Domain.UserAccess.Validations
{
    public class UserValidator : AbstractValidator<Usuario>
    {
        public UserValidator()
        {
            RuleFor(product => product.Nome)
                .NotEmpty().WithMessage(Mensagem.FieldRequired.GetDescription(Campo.Name))
                .Length(1, 100);
        }
    }
}
