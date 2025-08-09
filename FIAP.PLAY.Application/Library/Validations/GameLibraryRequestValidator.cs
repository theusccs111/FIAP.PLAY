using FIAP.PLAY.Application.Library.Resource.Request;
using FluentValidation;

namespace FIAP.PLAY.Application.Library.Validations
{
    public class GameLibraryRequestValidator : AbstractValidator<GameLibraryRequest>
    {
        public GameLibraryRequestValidator() 
        {
            RuleFor(request => request.Library)
                .NotNull().WithMessage("Biblioteca é obrigatória.")
                .SetValidator(new LibraryRequestValidator());

            RuleFor(request => request.Game)
                .NotNull().WithMessage("Jogo é obrigatório.")
                .SetValidator(new GameRequestValidator())
                .WithMessage("Jogo inválido.");
        }
    }
}
