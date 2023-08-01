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
  
    public const string Basic = "basic";
    public const string Lark = "lark";
    public const string Goods = "goods";
    public const string Design = "design";
    public const string User = "user";
    public const string Common = "common";
}

public static class SwaggerGroups
{
    private static Dictionary<string, OpenApiInfo> Modules => new Dictionary<string, OpenApiInfo>()
    {
        {
            SwaggerGrouping.Basic, new OpenApiInfo { Title = "基础", Version = "v1" }
        },
        {
            SwaggerGrouping.Goods, new OpenApiInfo { Title = "商品", Version = "v1" }
        },
        {
            SwaggerGrouping.Design, new OpenApiInfo { Title = "设计", Version = "v1" }
        },
        {
            SwaggerGrouping.Lark, new OpenApiInfo { Title = "飞书", Version = "v1" }
        },
        { 
            SwaggerGrouping.User, new OpenApiInfo{ Title = "用户", Version = "v1" }
        },
        {
            SwaggerGrouping.Common, new OpenApiInfo{ Title = "通用", Version = "v1" }
        }
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