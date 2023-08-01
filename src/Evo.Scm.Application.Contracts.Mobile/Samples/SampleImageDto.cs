using System;
using Volo.Abp.Application.Dtos;

namespace Evo.Scm.Samples;

/// <summary>
/// 样衣图片Dto
/// </summary>
public class UploadSampleImageDto : EntityDto<Guid>
{
    /// <summary>
    /// 颜色Id(来源设计档案)
    /// </summary>
    public Guid? ColorId { get; private set; }
    /// <summary>
    /// 颜色名称(来源设计档案)
    /// </summary>
    public string? ColorName { get; private set; }
    /// <summary>
    /// 正面图片
    /// </summary>
    public string? ImageFront { get; private set; }
    /// <summary>
    /// 背面图片
    /// </summary>
    public string? ImageBack { get; private set; }
}
