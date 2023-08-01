using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Evo.Scm;

/// <summary>
/// 用于枚举描述的生成
/// </summary>
public class EnumDocumentFilter : IDocumentFilter
{
    /// <summary>
    /// 实现IDocumentFilter接口的Apply函数
    /// </summary>
    /// <param name="swaggerDoc"></param>
    /// <param name="context"></param>
    public void Apply(Microsoft.OpenApi.Models.OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        Dictionary<string, Type> dict = GetAllEnum();

        foreach (var item in swaggerDoc.Components.Schemas)
        {
            var property = item.Value;
            var typeName = item.Key;
            Type itemType = null;
            if (property.Enum != null && property.Enum.Count > 0)
            {
                if (dict.ContainsKey(typeName))
                {
                    itemType = dict[typeName];
                }
                else
                {
                    itemType = null;
                }

                List<OpenApiInteger> list = new List<OpenApiInteger>();
                foreach (var val in property.Enum)
                {
                    list.Add((OpenApiInteger)val);
                }

                property.Description += DescribeEnum(itemType, list);
            }
        }
    }

    private static Dictionary<string, Type> GetAllEnum()
    {
        Dictionary<string, Type> dict = new Dictionary<string, Type>();

        Assembly ass = Assembly.Load("Evo.Scm.Domain.Shared"); //枚举所在的命名空间的xml文件名，枚举都放在Evo.Scm.Domain.Shared层里（类库）
        Type[] types = ass.GetTypes();
        foreach (Type item in types)
        {
            if (item.IsEnum)
            {
                dict.Add(item.FullName, item);
            }
        }
        
        ass = Assembly.Load("Evo.Scm.Application.Contracts"); //枚举所在的命名空间的xml文件名，枚举都放在Evo.Scm.Application.Contracts层里（类库）
        types = ass.GetTypes();
        foreach (Type item in types)
        {
            if (item.IsEnum)
            {
                dict.Add(item.FullName, item);
            }
        }

        return dict;
    }

    private static string DescribeEnum(Type type, List<OpenApiInteger> enums)
    {
        var enumDescriptions = new List<string>();
        foreach (var item in enums)
        {
            if (type == null) continue;
            var value = Enum.Parse(type, item.Value.ToString());
            var desc = GetDescription(type, value);

            if (string.IsNullOrEmpty(desc))
                enumDescriptions.Add($"{item.Value.ToString()}：{Enum.GetName(type, value)}；");
            else
                enumDescriptions.Add($"{item.Value.ToString()}：{Enum.GetName(type, value)}，{desc}；");
        }

        return $"<br><div>{Environment.NewLine}{string.Join("<br/>" + Environment.NewLine, enumDescriptions)}</div>";
    }

    private static string GetDescription(Type t, object value)
    {
        foreach (MemberInfo mInfo in t.GetMembers())
        {
            if (mInfo.Name == t.GetEnumName(value))
            {
                foreach (Attribute attr in Attribute.GetCustomAttributes(mInfo))
                {
                    if (attr.GetType() == typeof(DescriptionAttribute))
                    {
                        return ((DescriptionAttribute)attr).Description;
                    }
                }
            }
        }

        return string.Empty;
    }
}