using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PaymentTransaction.Middleware
{
    public class CombinedAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ApiKeyHeaderName = "x-api-key";

        public CombinedAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration config)
        {
            var expectedApiKey = config["ApiKey"];
            var providedApiKey = context.Request.Headers[ApiKeyHeaderName].FirstOrDefault();

            if (!string.IsNullOrEmpty(providedApiKey) && expectedApiKey == providedApiKey)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, "ApiKeyUser"),
                    new Claim(ClaimTypes.Role, "ApiKeyClient")
                };

                var identity = new ClaimsIdentity(claims, "ApiKey");
                context.User = new ClaimsPrincipal(identity);
            }
            else
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized: Provide a valid API key.");
                return;
            }

            await _next(context);
        }
    }
}
