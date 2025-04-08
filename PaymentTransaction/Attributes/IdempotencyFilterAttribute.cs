using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PaymentTransaction.Repositories;

namespace PaymentTransaction.Attributes
{
  public class IdempotencyFilter : IAsyncActionFilter
  {
      private readonly ITransactionRepository _transactionRepository;

      public IdempotencyFilter(ITransactionRepository transactionRepository)
      {
          _transactionRepository = transactionRepository;
      }

      public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
  {
      if (!context.HttpContext.Request.Headers.TryGetValue("Idempotency-Key", out var idempotencyKey) || string.IsNullOrWhiteSpace(idempotencyKey))
      {
          idempotencyKey = Guid.NewGuid().ToString();
      }

      // 👇 At this point, we know it's non-null and non-empty
      var existingTransaction = await _transactionRepository.GetByIdempotencyKeyAsync(idempotencyKey!);
      
      if (existingTransaction != null)
      {
          context.Result = new OkObjectResult(existingTransaction);
      }
      else
      {
          await next();
      }
  }
  }
}
