using System;

namespace Evo.Scm.Samples;

public class FillInSampleProduceImageDto
{
    /// <summary>
    /// 颜色Id(来源设计档案)
    /// </summary>
    public Guid ColorId { get; set; }
    /// <summary>
    /// 颜色名称(来源设计档案)
    /// </summary>
    public string ColorName { get; set; }
    /// <summary>
    /// 正面图片
    /// </summary>
    public string? ImageFront { get; set; }
    /// <summary>
    /// 背面图片
    /// </summary>
    public string? ImageBack { get; set; }
    /// <summary>
    /// 细节图片1
    /// </summary>
    public string ImageDetail1 { get; set; }
    /// <summary>
    /// 细节图片2
    /// </summary>
    public string ImageDetail2 { get; set; }
}
