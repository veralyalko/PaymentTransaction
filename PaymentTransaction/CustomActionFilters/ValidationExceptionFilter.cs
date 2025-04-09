using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PaymentTransaction.CustomActionFilters
{public class ValidationExceptionFilter : IExceptionFilter
  {
      public void OnException(ExceptionContext context)
      {
          if (context.Exception is ValidationException validationException)
          {
              var errors = validationException.Errors
                  .GroupBy(e => e.PropertyName)
                  .ToDictionary(
                      g => g.Key,
                      g => g.Select(e => e.ErrorMessage).ToArray());

              context.Result = new BadRequestObjectResult(new { errors });
              context.ExceptionHandled = true;
          }
      }
  }
}
