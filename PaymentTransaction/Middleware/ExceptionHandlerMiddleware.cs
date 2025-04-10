using System.Net;

namespace PaymentTransaction.Middleware
{
    public class ExceptionHandlerMiddleware 
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;
        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger,
            RequestDelegate next)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try{
                await next(httpContext);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();
                // Log Exception
                logger.LogError(ex, $"{errorId} : {ex.Message}");

                // Return custon error responce 
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";
                
                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong. We are looking into resolving this issue."
                };

                await httpContext.Response.WriteAsJsonAsync(error);
            }

        }
    }
}