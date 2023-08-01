using System;

namespace Evo.Scm.Samples;

/// <summary>
/// 确认齐料Dto
/// </summary>
public class ConfirmMaterialDto : SampleIdsDto
{
    /// <summary>
    /// 物料准备时间(当前登录人为物料准备人时传null)
    /// </summary>
    public DateTime? MaterialConfirmTime { get; set; }
    /// <summary>
    /// 物料准备人姓名(当前登录人为物料准备人时传null)
    /// </summary>
    public string MaterialConfirmerName { get; set; }
}
