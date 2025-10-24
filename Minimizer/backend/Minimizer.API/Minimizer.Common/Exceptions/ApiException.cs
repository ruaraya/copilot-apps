using Microsoft.AspNetCore.Mvc.ModelBinding;
using Minimizer.Common.Extensions;
using Minimizer.Common.Models.Exceptions;
using System.Net;

namespace Minimizer.Common.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; init; }

        public IEnumerable<ValidationError> ValidationErrors { get; init; }
        public ApiException(ModelStateDictionary modelState,
            int statusCode = (int)HttpStatusCode.BadRequest)
        {
            ValidationErrors = modelState.AllErrors();
            StatusCode = statusCode;
        }
    }
}
