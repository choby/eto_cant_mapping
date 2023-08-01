
namespace Evo.Scm.Kendo;

public class KendoResult<T> : global::Kendo.Mvc.UI.DataSourceResult
{
    public new IEnumerable<T> Data { get; set; }
}

internal  static class DataSourceResultExtension
{
    internal static KendoResult<T> GetKendoResult<T>(this global::Kendo.Mvc.UI.DataSourceResult result)
    {
        return new KendoResult<T>
        {
            Total = result.Total,
            AggregateResults = result.AggregateResults,
            Errors = result.Errors,
            Data = result.Data as IEnumerable<T>
        };
    }
}