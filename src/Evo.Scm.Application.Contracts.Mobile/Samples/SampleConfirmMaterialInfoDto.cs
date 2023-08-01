using System;
using Volo.Abp.Application.Dtos;

namespace Evo.Scm.Samples;

public class SampleConfirmMaterialInfoDto : EntityDto<Guid>
{
    /// <summary>
    /// 物料准备时间
    /// </summary>
    public DateTime? MaterialConfirmTime { get; set; }
    /// <summary>
    /// 物料准备人姓名
    /// </summary>
    public string? MaterialConfirmerName { get; set; }
}
