using System;
using System.ComponentModel.DataAnnotations;

namespace Evo.Scm.Positions;

public class SearchUserFromPosistionDto
{
    /// <summary>
    /// 岗位Code
    /// </summary>
    [Required]
    public string PositionCode { get; set; }
    /// <summary>
    /// 品牌Id
    /// </summary>
    public Guid? BrandId { get; set; }
    /// <summary>
    /// 页数
    /// </summary>
    public int? PageIndex { get; set; }
    /// <summary>
    /// 页大小
    /// </summary>
    public int? PageSize { get; set; }
}
