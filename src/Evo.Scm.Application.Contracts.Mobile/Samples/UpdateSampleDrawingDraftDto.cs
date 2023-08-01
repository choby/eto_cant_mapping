using System;
using System.ComponentModel.DataAnnotations;

namespace Evo.Scm.Samples;

/// <summary>
/// 花稿准备Dto
/// </summary>
public class UpdateSampleDrawingDraftDto
{
    /// <summary>
    /// 完成人名称
    /// </summary>
    [Required(ErrorMessage = "请填写花稿完成人")]
    public string DrawingDraftConfirmerName { get; set; }
    /// <summary>
    /// 完成时间
    /// </summary>
    [Required(ErrorMessage = "请填写完成日期")]
    public DateTime? DrawingDraftConfirmTime { get; set; }
    /// <summary>
    /// 图案效果图
    /// </summary>
    public string? DrawingDraftImage { get; set; }
}