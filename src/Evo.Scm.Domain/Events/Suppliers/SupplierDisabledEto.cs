using Evo.Scm.Suppliers;

namespace Evo.Scm.Events;

/// <summary>
/// 供应商账号禁用
/// </summary>
public class SupplierDisabledEto
{
    public SupplierDisabledEto(Supplier supplier)
    {
        Supplier = supplier;
    }

    /// <summary>
    /// 供应商
    /// </summary>
    public Supplier Supplier { get; private set; }
}
