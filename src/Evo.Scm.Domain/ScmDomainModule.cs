using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;
using EasyAbp.NotificationService;
using Volo.Abp.AutoMapper;

namespace Evo.Scm;

[DependsOn(
    typeof(ScmDomainSharedModule),
    typeof(AbpAuditLoggingDomainModule),
    typeof(AbpBackgroundJobsDomainModule),
    typeof(AbpAutoMapperModule),
    //typeof(AbpTenantManagementDomainModule),
    typeof(AbpEmailingModule),
    typeof(NotificationServiceDomainModule)
)]
public class ScmDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<ScmDomainModule>(); });
#if DEBUG
        context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
#endif
    }
}