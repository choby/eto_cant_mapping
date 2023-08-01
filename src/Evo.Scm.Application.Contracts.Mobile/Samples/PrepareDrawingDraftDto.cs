using System;
using Volo.Abp.Application.Dtos;

namespace Evo.Scm.Samples;

public class PrepareDrawingDraftDto : EntityDto<Guid>
{
    /// <summary>
    /// 完成人名称
    /// </summary>
    public string DrawingDraftConfirmerName { get; set; }
    /// <summary>
    /// 完成时间
    /// </summary>
    public DateTime? DrawingDraftConfirmTime { get; set; }
    /// <summary>
    /// 图案效果图
    /// </summary>
    public string DrawingDraftImage { get; set; }
}