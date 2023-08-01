using Evo.Scm.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Evo.Scm.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ScmController : AbpControllerBase
{
    protected ScmController()
    {
        LocalizationResource = typeof(ScmResource);
    }
}
