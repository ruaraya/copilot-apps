using Microsoft.AspNetCore.Mvc.ModelBinding;
using Minimizer.Common.Models.Exceptions;

namespace Minimizer.Common.Extensions
{
    public static class ModelStateExtension
    {
        public static IEnumerable<ValidationError> AllErrors(this ModelStateDictionary modelState) =>
            modelState.Keys.SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage))).ToList();
    }
}