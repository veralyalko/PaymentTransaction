using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace PaymentTransaction.Attributes
{
    public class AddIdempotencyKeyHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check if the action method has the [RequiresIdempotencyKeyHeader] attribute
            var hasIdempotencyAttribute = context.MethodInfo
                .GetCustomAttribute<RequiresIdempotencyKeyHeaderAttribute>() != null;

            if (!hasIdempotencyAttribute)
                return; // Don't add the header if the attribute is missing

            operation.Parameters ??= new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Idempotency-Key",
                In = ParameterLocation.Header,
                Required = true, // Set to true if your API expects this header
                Description = "A unique key to ensure idempotent operations",
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Example = new Microsoft.OpenApi.Any.OpenApiString("123e4567-e89b-12d3-a456-426614174000")
                }
            });
        }
    }
}


