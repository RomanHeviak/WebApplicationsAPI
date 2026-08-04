using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApplicationAPI.Swagger
{
    public class AuthorizeCheckDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var apiDescription in context.ApiDescriptions)
            {
                var metadata = apiDescription.ActionDescriptor.EndpointMetadata;

                var hasAuthorize = metadata.OfType<AuthorizeAttribute>().Any();
                var allowAnonymous = metadata.OfType<AllowAnonymousAttribute>().Any();

                if (!hasAuthorize || allowAnonymous)
                {
                    continue;
                }

                var path = "/" + apiDescription.RelativePath?.TrimEnd('/');
                if (!swaggerDoc.Paths.TryGetValue(path, out var pathItem))
                {
                    continue;
                }

                if (string.IsNullOrEmpty(apiDescription.HttpMethod))
                {
                    continue;
                }

                var httpMethod = HttpMethod.Parse(apiDescription.HttpMethod);

                if (pathItem.Operations is null ||
                    !pathItem.Operations.TryGetValue(httpMethod, out var operation))
                {
                    continue;
                }

                operation.Security =
                [
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecuritySchemeReference("Bearer", swaggerDoc),
                            new List<string>()
                        }
                    }
                ];
            }
        }
    }
}
