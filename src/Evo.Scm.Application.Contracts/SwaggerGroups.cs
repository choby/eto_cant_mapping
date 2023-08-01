using System.Collections.Generic;
using Masuit.Tools;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Evo.Scm;

/// <summary>
/// 分组
/// </summary>
public static class SwaggerGrouping
{
  
    
}

public static class SwaggerGroups
{
    private static Dictionary<string, OpenApiInfo> Modules => new Dictionary<string, OpenApiInfo>()
    {
        
    };
    public static void SwaggerDocGroups(this SwaggerGenOptions options)
    {
        Modules.ForEach(module =>
        {
            options.SwaggerDoc(module.Key, module.Value);
        });
    }

    public static void SwaggerEndpointGroups(this SwaggerUIOptions options)
    {
        Modules.ForEach(module =>
        {
            options.SwaggerEndpoint($"/swagger/{module.Key}/swagger.json", module.Value.Title);
        });
    }
}