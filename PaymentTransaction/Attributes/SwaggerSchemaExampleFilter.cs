using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json;

/// <summary>
/// A filter that applies examples to the Swagger schema for properties annotated with <see cref="SwaggerSchemaExampleAttribute"/>.
/// </summary>
public class SwaggerSchemaExampleFilter : ISchemaFilter
{
    /// <summary>
    /// Applies the example values from <see cref="SwaggerSchemaExampleAttribute"/> to the schema for each property.
    /// </summary>
    /// <param name="schema">The OpenAPI schema to which the examples will be applied.</param>
    /// <param name="context">The context that provides access to the property information.</param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        foreach (var property in context.Type.GetProperties())
        {
            var exampleAttribute = property.GetCustomAttribute<SwaggerSchemaExampleAttribute>();
            if (exampleAttribute != null && schema.Properties.ContainsKey(property.Name))
            {
                schema.Properties[property.Name].Example = OpenApiAnyFactory.CreateFromJson(JsonSerializer.Serialize(exampleAttribute.Example));
            }
        }
    }
}
