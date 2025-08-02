using FIAP.PLAY.Application.Shared.Resource;
using FIAP.PLAY.Domain.Shared.Exceptions;
using FIAP.PLAY.Domain.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace FIAP.PLAY.Web.Filters.Shared
{
    [AttributeUsage(AttributeTargets.All)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var resultado = new Result<object>();
            if (context.Exception is ValidationException)
            {
                var validationException = (ValidationException)context.Exception;
                string[] failures = validationException.Failures.ConvertDictionaryToArray();

                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                resultado = new Result<object>
                {
                    Success = false,
                    Message = validationException.Message,
                    MessageDetail = string.Join(";",failures),
                    Data = null
                };

                context.Result = new JsonResult(resultado);
                return;
            }

            var code = HttpStatusCode.InternalServerError;

            if (context.Exception is NotFoundException)
            {
                code = HttpStatusCode.NotFound;

            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)code;

            resultado = new Result<object>
            {
                Success = false,
                Message = context.Exception.Message,
                //MessageDetail = exception.InnerException?.Message,
                Data = null
            };

            context.Result = new JsonResult(resultado);
        }
    }
}
