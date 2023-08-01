using System;

namespace Evo.Scm.DesignStyles;

public class DesignStyleSelectOptionDto
{
    public DesignStyleSelectOptionDto(Guid id, string sn)
    {
        this.Id = id;
        this.Sn = sn;
    }

    /// <summary>
    /// 设计款Id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// 设计款号
    /// </summary>
    public string Sn { get; set; }
    /// <summary>
    /// 品牌名称
    /// </summary>
    public string BrandName { get; set; }
}
