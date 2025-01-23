using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace repassAPI.Utils;

public class SecurityRequirementsFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorize = context.MethodInfo.DeclaringType?.GetCustomAttributes<AuthorizeAttribute>(true).Any() == true ||
                           context.MethodInfo.GetCustomAttributes<AuthorizeAttribute>(true).Any();

        if (!hasAuthorize) { return; }
        
        var securityScheme = new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer" 
            }
        };

        operation.Security = new List<OpenApiSecurityRequirement>();
        operation.Security.Add(new OpenApiSecurityRequirement
        {
            [securityScheme] = Array.Empty<string>() 
        });
    }
}