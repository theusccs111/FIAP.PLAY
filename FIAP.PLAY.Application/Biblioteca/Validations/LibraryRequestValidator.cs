using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using FluentValidation;

namespace FIAP.PLAY.Application.Biblioteca.Validations
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
