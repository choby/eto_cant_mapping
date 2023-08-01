using System;
using System.ComponentModel.DataAnnotations;

namespace Evo.Scm.Samples;

public class FinishCutDto : SampleIdsDto
{
    /// <summary>
    /// 裁版师姓名
    /// </summary>
    [Required]
    public string CutterName { get; set; }
    /// <summary>
    /// 裁版完成时间
    /// </summary>
    [Required]
    public DateTime CutFinishTime { get; set; }
    /// <summary>
    /// 版布幅宽
    /// </summary>
    public decimal? CutFabricWidth { get; set; }
    /// <summary>
    /// 裁版系数
    /// </summary>
    public decimal? CutCoefficient { get; set; }
}

