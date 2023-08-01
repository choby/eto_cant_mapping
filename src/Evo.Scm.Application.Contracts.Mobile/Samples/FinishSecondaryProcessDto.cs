using System;
using System.ComponentModel.DataAnnotations;

namespace Evo.Scm.Samples;

public class FinishSecondaryProcessDto : SampleIdsDto
{
    /// <summary>
    /// 二次加工跟进人
    /// </summary>
    [Required]
    public string SecondaryProcessFollowerName { get; set; }
    /// <summary>
    /// 二次加工完成时间
    /// </summary>
    [Required]
    public DateTime SecondaryProcessFinishTime { get; set; }
    /// <summary>
    /// 加工厂id
    /// </summary>
    public Guid? SupplierId { get; set; }
}
