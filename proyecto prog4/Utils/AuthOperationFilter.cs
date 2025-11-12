using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace proyecto_prog4.Utils
{
    public class AuthOperationFilter : IOperationFilter
    {

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var Attributtes = context.ApiDescription.CustomAttributes();
            var isAuthorize = Attributtes.Any(attr => attr.GetType() == typeof(AuthorizeAttribute));
            var isAllowAnonymous = Attributtes.Any(attr => attr.GetType() == typeof(AllowAnonymousAttribute));

            if (!isAuthorize || isAllowAnonymous) return;

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    [
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Token"}
                        }

                    ] = new string[] { }
                }
            };
        }
    }
}
