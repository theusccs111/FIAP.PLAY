using FIAP.PLAY.Domain.Entities;
using FIAP.PLAY.Domain.Enums;
using FIAP.PLAY.Domain.Extensions;
using FluentValidation;

namespace FIAP.PLAY.Domain.Validations
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(product => product.Nome)
                .NotEmpty().WithMessage(Mensagem.FieldRequired.GetDescription(Campo.Name))
                .Length(1, 100)
                ;



        }
    }
}
