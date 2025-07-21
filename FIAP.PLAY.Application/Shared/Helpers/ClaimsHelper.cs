using Microsoft.AspNetCore.Http;
using System.Linq;

namespace FIAP.PLAY.Application.Shared.Helpers
{
    public static class ClaimsHelper
    {
        public static string ObterInformacaoDoClaims(HttpContext context, string key)
        {
            string? ret = "";
            if (context != null)
            {
                ret = context.User.Claims.FirstOrDefault(c => c.Type == key)?.Value;
            }

            return ret;
        }

    }
}
