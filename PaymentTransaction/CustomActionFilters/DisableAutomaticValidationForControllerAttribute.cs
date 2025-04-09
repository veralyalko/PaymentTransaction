using Microsoft.AspNetCore.Mvc.Filters;

namespace PaymentTransaction.CustomActionFilters
{
  public class DisableAutomaticValidationForControllerAttribute : ActionFilterAttribute
  {
      public override void OnActionExecuting(ActionExecutingContext context)
      {
          context.ModelState.Clear(); // This will suppress the automatic validation
          base.OnActionExecuting(context);
      }
  }
}