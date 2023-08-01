using System;
using System.ComponentModel.DataAnnotations;

namespace Evo.Scm.Samples;

public class SampleIdsDto
{
    /// <summary>
    /// 样衣Id数组
    /// </summary>
    [Required]
    public Guid[] Ids { get; set; }
}
