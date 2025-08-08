using FIAP.PLAY.Application.UserAccess.Resource.Request;
using FIAP.PLAY.Domain.Shared.Enums;
using FIAP.PLAY.Domain.Shared.Extensions;
using FluentValidation;

namespace FIAP.PLAY.Application.UserAccess.Validations
{
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty().WithMessage(Message.FieldRequired.GetDescription(Field.Name))
                .Length(3, 100).WithMessage("Nome deve ter entre 3 e 100 caracteres.");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage(Message.FieldRequired.GetDescription(Field.Email))
                .EmailAddress().WithMessage("Email informado deve ser válido");

            RuleFor(user => user.PasswordHash)
                .NotEmpty().WithMessage("Senha não pode ser vazia")
                .MinimumLength(8).WithMessage("A senha deve conter pelo menos 8 caracteres.")
                .MaximumLength(16).WithMessage("A senha não pode exeder 16 caracteres.")
                .Matches(@"[A-Z]+").WithMessage("A senha deve conter pelo menos uma letra maiúscula.")
                .Matches(@"[a-z]+").WithMessage("A senha deve conter pelo menos uma letra minúscula.")
                .Matches(@"[0-9]+").WithMessage("A senha deve conter pelo menos um número.")
                .Matches(@"[\!\?\*\.]+").WithMessage("A senha deve conter pelo menos um desses caracteres (!? *.).");
        }
    }
}
