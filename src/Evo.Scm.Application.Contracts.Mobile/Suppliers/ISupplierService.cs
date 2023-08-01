namespace Evo.Scm.Suppliers;

public interface ISupplierService
{
    Task<List<SupplierSelectOptionDto>> GetSupplierSelectOptionAsync(string sn, string serviceClass);
}
