using EasyAbp.NotificationService.Provider.PrivateMessaging;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;

namespace Evo.Scm;

[DependsOn(
    typeof(AbpFluentValidationModule),
    typeof(ScmDomainSharedModule),
    typeof(AbpObjectExtendingModule)
)]
public class ScmApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        ScmDtoExtensions.Configure();
    }
}
