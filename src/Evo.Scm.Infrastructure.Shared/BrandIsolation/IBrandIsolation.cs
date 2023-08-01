using System.Diagnostics.CodeAnalysis;

namespace Evo.Scm.BrandIsolation;

/// <summary>
/// 品牌数据隔离
/// </summary>
public interface IBrandIsolation
{
    /// <summary>
    /// 品牌id，必填
    /// </summary>
    [NotNull]
    Guid BrandId { get; }
}

