using FIAP.PLAY.Domain.Enums;
using FIAP.PLAY.Domain.Extensions;

namespace FIAP.PLAY.Domain.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException() : base(Mensagem.ErrorSystem.GetDescription())
        {
            Failures = new Dictionary<string, string[]>();
        }

        public ValidationException(string key, string message) : this()
        {
            Failures.Add(key, new string[1] { message });
        }
        public ValidationException(string key, string[] message) : this()
        {
            Failures.Add(key, message);
        }

        public ValidationException(List<FluentValidation.Results.ValidationFailure> failures)
            : this()
        {
            var propertyNames = failures
                .Select(e => e.PropertyName)
                .Distinct();

            foreach (var propertyName in propertyNames)
            {
                var propertyFailures = failures
                    .Where(e => e.PropertyName == propertyName)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                Failures.Add(propertyName, propertyFailures);
            }
        }

        public IDictionary<string, string[]> Failures { get; }
    }
}
