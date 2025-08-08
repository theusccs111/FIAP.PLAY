using FIAP.PLAY.Application.Promotions.Resources.Request;
using FIAP.PLAY.Domain.Shared.Enums;
using FIAP.PLAY.Domain.Shared.Extensions;
using FluentValidation;

namespace FIAP.PLAY.Application.Promotions.Validations
{
    public class CampaignRequestValidator : AbstractValidator<CampaignRequest>
    {
        public CampaignRequestValidator()
        {
            RuleFor(user => user.Description)
                .NotEmpty().WithMessage(Message.FieldRequired.GetDescription(Field.Description))
                .Length(3, 100).WithMessage("Descrição deve ter entre 3 e 100 caracteres.");

            RuleFor(c => c.StartDate)
                .NotEmpty().WithMessage(Message.FieldRequired.GetDescription(Field.StartDate))
                .LessThan(c => c.EndDate).WithMessage("Data de início deve ser menor que a data de fim.");

            RuleFor(c => c.EndDate)
                .NotEmpty().WithMessage(Message.FieldRequired.GetDescription(Field.EndDate))
                .GreaterThan(c => c.StartDate).WithMessage("Data de fim deve ser maior que a data de início.");

        }
    }
}
