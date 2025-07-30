using FIAP.PLAY.Domain.Biblioteca.Jogos.Entities;
using FluentValidation;

namespace FIAP.PLAY.Application.Biblioteca.Validations
{
    public class JogoValidator : AbstractValidator<Jogo>
    {
        public JogoValidator()
        {
            RuleFor(jogo => jogo.Titulo)
                .NotEmpty().WithMessage("Título não pode ser vazio.")
                .Length(1, 100).WithMessage("Título deve ter entre 3 e 100 caracteres.");
        }
    }
}
