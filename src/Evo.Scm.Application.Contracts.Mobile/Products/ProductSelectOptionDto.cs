using Evo.Scm.Brands;
using System;
using System.Collections.Generic;

namespace Evo.Scm.Products;

public class ProductSelectOptionDto
{
    public ProductSelectOptionDto(Guid id, string sn)
    {
        this.Id = id;
        this.Sn = sn;
    }

    /// <summary>
    /// 大货Id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// 大货款号
    /// </summary>
    public string Sn { get; set; }
}

public class ProductSelectOptionDetailDto : ProductSelectOptionDto
{
    public ProductSelectOptionDetailDto(Guid id, string sn) : base(id, sn)
    {
    }

    /// <summary>
    /// 波段
    /// </summary>
    public string? Waveband { get; set; }
    /// <summary>
    /// 年份
    /// </summary>
    public string? Year { get; set; }
    /// <summary>
    /// 季节
    /// </summary>
    public string? Season { get; set; }
    /// <summary>
    /// 图片路径,   主图/实物图
    /// </summary>
    public string? Image1 { get; set; }
    /// <summary>
    /// 品牌类型
    /// </summary>
    public IEnumerable<string[]> ProduceOrderQuantitieConvert { get; set; }
}
