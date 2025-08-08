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
    public class PromotionGameRequestValidator : AbstractValidator<PromotionGameRequest>
    {
        public PromotionGameRequestValidator()
        {
            RuleFor(p => p.PromotionId)
                .GreaterThan(0).WithMessage(Message.FieldRequired.GetDescription(Field.PromotionId));

            RuleFor(p => p.GameId)
                .GreaterThan(0).WithMessage(Message.FieldRequired.GetDescription(Field.GameId));


        }
    }
}
