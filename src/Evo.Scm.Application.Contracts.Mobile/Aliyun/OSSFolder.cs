
using System.Collections.Generic;


namespace Evo.Scm.Aliyun;

public class OSS
{
    public static Dictionary<string, string> Folders = new Dictionary<string, string>
    {
        { Features.Brand, Features.Brand.ToLower()},
        { Features.Option, Features.Option.ToLower()},
        { Features.NewStylePlan, Features.NewStylePlan.ToLower()},
        { Features.Selection, Features.Selection.ToLower()},
        { Features.Product, Features.Product.ToLower()},
        { Features.DesignStyle, Features.DesignStyle.ToLower()},
        { Features.Sample, Features.Sample.ToLower()},
        { Features.ProduceOrder, Features.ProduceOrder.ToLower()},
        { Features.Supplier, Features.Supplier.ToLower()},
        { Features.ProductDeliveryQt, Features.ProductDeliveryQt.ToLower()},
    };
}