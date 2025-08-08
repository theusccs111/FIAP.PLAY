using FIAP.PLAY.Application.Promotions.Resources.Request;
using FIAP.PLAY.Domain.Shared.Enums;
using FIAP.PLAY.Domain.Shared.Extensions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Application.Promotions.Validations
{
    public class PromotionRequestValidator : AbstractValidator<PromotionRequest>
    {
        public PromotionRequestValidator()
        {
            RuleFor(p => p.DiscountPercentage)
                .GreaterThan(0).WithMessage("O percentual de desconto deve ser maior que zero.")
                .LessThanOrEqualTo(100).WithMessage("O percentual de desconto não pode exceder 100%.");

            RuleFor(p => p.StartDate)
                .NotEmpty().WithMessage(Message.FieldRequired.GetDescription(Field.StartDate))
                .LessThan(p => p.EndDate).WithMessage("Data de início deve ser menor que a data de término.");

            RuleFor(p => p.EndDate)
                .NotEmpty().WithMessage(Message.FieldRequired.GetDescription(Field.EndDate))
                .GreaterThan(p => p.StartDate).WithMessage("Data de término deve ser maior que a data de início.");

            RuleFor(p => p.CampaignId)
                .GreaterThan(0).WithMessage(Message.FieldRequired.GetDescription(Field.CampaignId));


        }
    }
}
