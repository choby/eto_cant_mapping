using System;
using Volo.Abp.Application.Dtos;

namespace Evo.Scm.Samples;

public class SampleSecondaryProcessDto : EntityDto<Guid>
{
    //// <summary>
    /// 二次加工图片
    /// </summary>
    public string SecondaryProcessImage { get; set; }
    /// <summary>
    /// 二次加工信息数组，格式：
    /// [
    ///   {
    ///     "sampleSecondaryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///     "sampleSecondaryName": "二次工艺1"
    ///   }
    /// ]
    /// </summary>
    public SecondaryProcessSelectOptionInfoDto[] SecondaryProcessSelectOptionInfo { get; set; }
}

public class SecondaryProcessSelectOptionInfoDto
{
    /// <summary>
    /// 二次工艺Id
    /// </summary>
    public Guid SampleSecondaryId { get; set; }
    /// <summary>
    /// 二次工艺名称
    /// </summary>
    public string SampleSecondaryName { get; set; }
}
