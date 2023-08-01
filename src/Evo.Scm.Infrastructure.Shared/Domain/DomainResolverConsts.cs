using Volo.Abp.Data;

namespace Evo.Scm.Domain;

public static class DomainResolverConsts
{
    public const string ExtraPropertiesPropertyNamePrefix = "additionalProp";

    /// <summary>
    /// 获取扩展属性名，从1开始
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    public static string ExtraPropertiesPropertyName(int order)
    {
        return $"{ExtraPropertiesPropertyNamePrefix}{order}";
    }
    /// <summary>
    /// 指定的扩展属性字段是否有值
    /// </summary>
    /// <param name="domain"></param>
    /// <param name="proptertyName"></param>
    /// <returns></returns>
    public static bool HasExtraPropertiesPropertyValue(this IHasExtraProperties domain, string proptertyName)
    {
        return domain.ExtraProperties.ContainsKey(proptertyName) && !string.IsNullOrWhiteSpace(domain.ExtraProperties[proptertyName]?.ToString());
    }
}