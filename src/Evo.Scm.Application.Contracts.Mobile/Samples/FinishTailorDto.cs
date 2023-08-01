using System;
using System.ComponentModel.DataAnnotations;

namespace Evo.Scm.Samples;

/// <summary>
/// 完成车版Dto
/// </summary>
public class FinishTailorDto : SampleIdsDto
{
    /// <summary>
    /// 车版师姓名
    /// </summary>
    [Required]
    public string TailorName { get; set; }
    /// <summary>
    /// 车版完成时间
    /// </summary>
    [Required]
    public DateTime TailorFinishTime { get; set; }
    /// <summary>
    /// 车版件数
    /// </summary>
    [Required]
    public int TailorQuantity { get; set; }
    /// <summary>
    /// 车版难度系数
    /// </summary>
    public decimal? TailorCoefficient { get; set; }
}
