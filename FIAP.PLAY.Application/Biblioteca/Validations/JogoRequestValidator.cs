using FIAP.PLAY.Application.Biblioteca.Resource.Request;
using FluentValidation;

namespace FIAP.PLAY.Application.Biblioteca.Validations
{
    public class JogoRequestValidator : AbstractValidator<JogoRequest>
    {
        public JogoRequestValidator()
        {
            RuleFor(jogo => jogo.Titulo)
                .NotEmpty().WithMessage("Título não pode ser vazio.")
                .Length(3, 100).WithMessage("Título deve ter entre 3 e 100 caracteres.");

            RuleFor(jogo => jogo.Preco)
                .GreaterThan(0).WithMessage("Preço não pode ser inferior ou igual a zero");

            RuleFor(jogo => jogo.Genero).IsInEnum().WithMessage("Genero informado não existe");

            RuleFor(jogo => jogo.AnoLancamento)
                .LessThan(DateTime.Now.Year).WithMessage("Ano de lançamento não pode ser maior que o ano atual")
                .GreaterThan(1950).WithMessage("Ano de lançamento não pode ser menor que 1950");

            RuleFor(jogo => jogo.Desenvolvedora)
                .NotEmpty().WithMessage("Desenvolvedora não pode ser vazio.")
                .Length(3, 100).WithMessage("Desenvolvedora deve ter entre 3 e 100 caracteres.");
        }
    }
}
