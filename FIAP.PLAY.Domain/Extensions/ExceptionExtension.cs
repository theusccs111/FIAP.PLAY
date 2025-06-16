using System;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FIAP.PLAY.Domain.Extensions
{
    public static class ExceptionExtension
    {
        public static string ToExceptionMessage(this ModelStateDictionary modelState)
        {
            var message = new StringBuilder("Existem um ou mais erros de validação:\r\n");
            foreach (var value in modelState.Values)
            {
                if (value.Errors.Count > 0)
                    message.AppendLine(value.Errors[0].ErrorMessage);
            }
            return message.ToString().TrimEnd(Environment.NewLine.ToCharArray()).Replace(Environment.NewLine, "<br />");
        }
    }
}
