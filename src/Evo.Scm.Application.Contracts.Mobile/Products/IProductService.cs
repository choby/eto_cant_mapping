using Volo.Abp.Application.Services;

namespace Evo.Scm.Products;

public interface IProductService : IApplicationService
{
  
    Task<IEnumerable<ProductSelectOptionDetailDto>> GetProductSelectOptionDataAsync(string sn);
}
