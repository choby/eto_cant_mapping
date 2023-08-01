using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;

namespace Evo.Scm;

[DependsOn(
    typeof(ScmDomainSharedModule),
    typeof(AbpObjectExtendingModule),
    typeof(AbpFluentValidationModule)
)]
public class ScmApplicationContractsMobileModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        ScmDtoExtensions.Configure();
    }
}
