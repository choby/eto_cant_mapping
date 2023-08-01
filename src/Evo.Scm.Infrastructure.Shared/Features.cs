using JetBrains.Annotations;

namespace Evo.Scm;

/// <summary>
/// 功能名称集合
/// </summary>
public static class Features
{
    /// <summary>
    /// 品牌
    /// </summary>
    public const string Brand = "Brand";
    /// <summary>
    /// 选项
    /// </summary>
    public const string Option = "Option";
    /// <summary>
    /// 上新计划
    /// </summary>
    public const string NewStylePlan = "NewStylePlan";
    /// <summary>
    /// 选品
    /// </summary>
    public const string Selection = "Selection";
    /// <summary>
    /// 大货
    /// </summary>
    public const string Product = "Product";
    /// <summary>
    /// 设计款
    /// </summary>
    public const string DesignStyle = "DesignStyle";
    /// <summary>
    /// 样衣(开发)
    /// </summary>
    public const string Sample = "Sample";
    /// <summary>
    /// 批次号
    /// </summary>
    public const string ProduceOrder = "ProduceOrder";

    /// <summary>
    /// 供应商
    /// </summary>
    public const string Supplier = "Supplier";

    /// <summary>
    /// 大货预约单(发货单)
    /// </summary>
    public const string ProductDeliveryOrder = "ProductDeliveryOrder";
    /// <summary>
    /// 大货采购单
    /// </summary>
    public const string ProductPurchaseOrder = "ProductPurchaseOrder";
    /// <summary>
    /// 生产跟进异常
    /// </summary>
    public const string ProduceTrackingError = "ProduceTrackingError";
    /// <summary>
    /// 大货正品入库单
    /// </summary>
    public const string ProductStockInOrder = "ProductStockInOrder";
    /// <summary>
    /// 次品退货单
    /// </summary>
    public const string ProductDefectiveReturnOrder = "ProductDefectiveReturnOrder";
    /// <summary>
    /// 品检单
    /// </summary>
    public const string ProductDeliveryQt = "ProductDeliveryQt";
    /// <summary>
    /// 成品流水明细
    /// </summary>
    public const string ProductSettlementDetail = "ProductSettlementDetail";
    /// <summary>
    /// 物料基础信息
    /// </summary>
    public const string MaterialBasic = "MaterialBasic";
    /// <summary>
    /// 采购合同
    /// </summary>
    public const string ProductPurchaseContract = "ProductPurchaseContract";
    /// <summary>
    /// 报表
    /// </summary>
    public const string Report = "Report";

    /// <summary>
    /// 打印模板
    /// </summary>
    public const string PrintTemplate = "PrintTemplate";
    
    /// <summary>
    /// 大货物料跟踪
    /// </summary>
    public const string ProductMaterialRequirementTracking = "ProductMaterialRequirementTracking";
    
    /// <summary>
    /// 判断字段是否存在
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool Exists([NotNull] string name)
    {
        
        var fields = typeof(Features).GetFields();
        return fields.Select(s => s.Name.ToLower()).Contains(name.ToLower());
    }

    /// <summary> 
    /// 获取功能
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string? GetFeature([NotNull] string name)
    {
        var fields = typeof(Features).GetFields();
        var field = fields.Single(s => s.Name.ToLower() == name.ToLower());
        return field.GetRawConstantValue()?.ToString();


    }
}
