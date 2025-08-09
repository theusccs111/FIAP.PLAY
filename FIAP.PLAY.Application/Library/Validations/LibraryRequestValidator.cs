using FIAP.PLAY.Application.Library.Resource.Request;
using FluentValidation;

namespace FIAP.PLAY.Application.Library.Validations
{
    public class LibraryRequestValidator : AbstractValidator<LibraryRequest>
    {
        public LibraryRequestValidator()
        {
           
            RuleFor(request => request.UserId)
                .NotEmpty().WithMessage("UserId é obrigatório.")
                .GreaterThan(0).WithMessage("Usuário Inválido.");
        }
    }
}
