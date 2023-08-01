using Localization.Resources.AbpUi;
using Evo.Scm.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Evo.Scm;

[DependsOn(
    typeof(ScmApplicationContractsMobileModule)
    )]
public class ScmHttpApiMobileModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureLocalization();
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ScmResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }
}
