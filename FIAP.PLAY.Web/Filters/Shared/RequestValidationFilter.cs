using FIAP.PLAY.Domain.Shared.Exceptions;
using FIAP.PLAY.Domain.Shared.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FIAP.PLAY.Web.Filters.Shared
{
    public class RequestValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
            {
                throw new ValidationException("Erro" , filterContext.ModelState.ToExceptionMessage());
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}
