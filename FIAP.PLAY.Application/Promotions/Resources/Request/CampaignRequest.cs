using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Domain.Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIAP.PLAY.Application.Promotions.Resources.Request
{
    public sealed record CampaignRequest(string Description, DateTime StartDate, DateTime EndDate) : RequestBase;
}
