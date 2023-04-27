#pragma warning disable CS1591 
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Diary_Server
{
    public class AuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            if (/*!context.ApiDescription.RelativePath.Contains("auth")*/ true)
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Token",
                    In = ParameterLocation.Header,
                    //Description = "access token",
                    //Required = true,
                    //Schema = new OpenApiSchema
                    //{
                    //    Type = "String",
                    //    Default = new OpenApiString("Bearer ")
                    //}
                });
            }
        }
    }
}
#pragma warning restore CS1591 