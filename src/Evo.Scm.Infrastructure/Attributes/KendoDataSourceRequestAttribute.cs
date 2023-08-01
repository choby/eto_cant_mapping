using Evo.Scm.ModelBinders.DataSourceRequestModelBinder;
using Microsoft.AspNetCore.Mvc;

namespace Evo.Scm.Attributes;

public class KendoDataSourceRequestAttribute : ModelBinderAttribute
{
    public KendoDataSourceRequestAttribute() => this.BinderType = typeof (DataSourceRequestModelBinder);
}