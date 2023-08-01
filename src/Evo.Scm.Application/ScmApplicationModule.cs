using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.DistributedLocking;

namespace Evo.Scm;

[DependsOn(
    typeof(ScmDomainModule),
    typeof(ScmApplicationContractsModule),
    typeof(AbpDistributedLockingModule)
    )]
    public class ScmApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<ScmApplicationModule>();
        });
    }
}
