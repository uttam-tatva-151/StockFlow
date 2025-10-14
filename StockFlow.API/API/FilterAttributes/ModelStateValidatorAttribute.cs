using StockFlow.Common.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StockFlow.API.FilterAttributes;

public class ModelStateValidatorAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            List<string> errors = context.ModelState
                .Where(m => m.Value != null && m.Value.Errors.Count > 0)
                .SelectMany(m => m.Value!.Errors.Select(e => $"{m.Key}: {e.ErrorMessage}"))
                .ToList();

            if (errors.Count != 0)
                throw new ModelValidationException(errors);
        }
    }
}
