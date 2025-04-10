using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PaymentTransaction
{
    public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider apiVersionDescriptionProvider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            this.apiVersionDescriptionProvider = apiVersionDescriptionProvider;
        }

        // Leave this empty to avoid double-registration
        public void Configure(string? name, SwaggerGenOptions options)
        {
            // Do nothing here
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
            }
        }

        private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = "YourVersionAPI",
                Version = description.ApiVersion.ToString(),
                Description = "Versioned API documentation"
            };

            if (description.IsDeprecated)
            {
                info.Description += " - This version is deprecated.";
            }

            return info;
        }
    }
}
