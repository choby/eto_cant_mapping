using AutoMapper;
using Evo.Scm.Suppliers;
using Evo.Scm.Domain;

namespace Evo.Scm;

public class ScmDomainAutoMapperProfile : Profile
{
    public ScmDomainAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Supplier, SupplierEto>();
        
    }
}
