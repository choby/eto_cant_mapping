using Volo.Abp.Modularity;
using Volo.Abp.BlobStoring.Aliyun;

namespace Evo.Scm;

[DependsOn(
    typeof(ScmDomainModule)
)]
public class ScmEventHandlersModule : AbpModule
{
    
}
