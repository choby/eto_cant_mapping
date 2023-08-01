
using Evo.Scm.Brands;

namespace Evo.Scm.BrandIsolation;

public class CacheBrandItem : IBrand
{
   
    public Guid Id { get; set; }
    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 品牌名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 图片地址
    /// </summary>
    public string Image { get; set; }

    /// <summary>
    /// 备注/描述
    /// </summary>
    public string Remark { get; set; }



    /// <summary>
    /// 采购主体（来源于选项表）
    /// </summary>
    public string PurchasingEntity { get; set; }
   

    public Guid? TenantId { get; set; }


    /// <summary>
    /// 事业部
    /// </summary>
    public string BusinessUnit { get; set; }
   
    /// <summary>
    /// 英文名
    /// </summary>
    public string? EnglishName { get; set; }
  

   
}
