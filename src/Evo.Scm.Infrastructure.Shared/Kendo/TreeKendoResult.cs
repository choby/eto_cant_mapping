namespace Evo.Scm.Kendo;

public class TreeKendoResult<T> : global::Kendo.Mvc.UI.TreeDataSourceResult
{
    public new IEnumerable<T> Data { get; set; }

}

internal  static class TreeDataSourceResultExtension
{
    internal static TreeKendoResult<T> GetTreeKendoResult<T>(this global::Kendo.Mvc.UI.TreeDataSourceResult result)
    {
        return new TreeKendoResult<T>
        {
            Total = result.Total,
            AggregateResults = result.AggregateResults,
            Errors = result.Errors,
            Data = result.Data as IEnumerable<T>
        };
    }
}