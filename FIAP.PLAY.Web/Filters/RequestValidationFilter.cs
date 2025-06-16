using FIAP.PLAY.Domain.Exceptions;
using FIAP.PLAY.Domain.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FIAP.PLAY.Web.Filters
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
