using System;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Evo.Scm.Samples;

public class FillInSecondaryProcessInfoDto
{
    /// <summary>
    /// 二次加工图片
    /// </summary>
    public string? SecondaryProcessImage { get; set; }
    /// <summary>
    /// 二次加工信息数组，格式：
    /// [
    ///   {
    ///     "sampleSecondaryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///     "sampleSecondaryName": "二次工艺1"
    ///   }
    /// ]
    /// </summary>
    public FillInSecondaryProcessSelectOptionInfoDto[] SecondaryProcessSelectOptionInfo { get; set; }
}

/// <summary>
/// 创建二次工艺信息Dto
/// </summary>
public class FillInSecondaryProcessSelectOptionInfoDto
{
    /// <summary>
    /// 二次工艺Id
    /// </summary>
    [Required]
    public Guid SampleSecondaryId { get; set; }
    /// <summary>
    /// 二次工艺名称
    /// </summary>
    [Required]
    public string SampleSecondaryName { get; set; }
}
