using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using FluentValidation;

namespace FIAP.PLAY.Application.Biblioteca.Validations
{
    public class GameRequestValidator : AbstractValidator<GameRequest>
    {
        public GameRequestValidator()
        {
            RuleFor(jogo => jogo.Title)
                .NotEmpty().WithMessage("Título não pode ser vazio.")
                .Length(3, 100).WithMessage("Título deve ter entre 3 e 100 caracteres.");

            RuleFor(jogo => jogo.Price)
                .GreaterThan(0).WithMessage("Preço não pode ser inferior ou igual a zero");

            RuleFor(jogo => jogo.Genre).IsInEnum().WithMessage("Genero informado não existe");

            RuleFor(jogo => jogo.YearLaunch)
                .LessThan(DateTime.Now.Year).WithMessage("Ano de lançamento não pode ser maior que o ano atual")
                .GreaterThan(1950).WithMessage("Ano de lançamento não pode ser menor que 1950");

            RuleFor(jogo => jogo.Developer)
                .NotEmpty().WithMessage("Desenvolvedora não pode ser vazio.")
                .Length(3, 100).WithMessage("Desenvolvedora deve ter entre 3 e 100 caracteres.");
        }
    }
}
