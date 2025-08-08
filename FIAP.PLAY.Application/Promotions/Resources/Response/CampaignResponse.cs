using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Domain.UserAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Application.Promotions.Resources.Response
{
    public sealed record CampaignResponse(long Id, string Description, DateTime StartDate, DateTime EndDate) : ResponseBase;
}
