using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ordering.Application.Exceptions
{
    public class ValidationException: ApplicationException
    {
        public ValidationException(): base("One or more validation failure have occured.")
        {
            Errors = new Dictionary<string, string[]>();
        }
        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroups => failureGroups.Key, failureGroups => failureGroups.ToArray());
        }

        public IDictionary<string, string[]>Errors { get; }
    }
}
