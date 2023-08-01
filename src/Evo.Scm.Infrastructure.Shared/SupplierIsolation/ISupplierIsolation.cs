
using JetBrains.Annotations;

namespace Evo.Scm.SupplierIsolation;

/// <summary>
/// 供应商数据隔离
/// </summary>
public interface ISupplierIsolation<TValue>
{
    TValue SupplierId { get; }
}













