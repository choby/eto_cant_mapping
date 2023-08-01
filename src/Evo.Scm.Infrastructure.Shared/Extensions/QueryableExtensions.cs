using System.Data;
using System.Linq.Expressions;
using System.Net;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Volo.Abp.Domain.Entities;
using Evo.Scm.Kendo;
using Kendo.Mvc;
using Kendo.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Evo.Scm;

/// <summary>
/// Provides extension methods to process DataSourceRequest.
/// </summary>
public static class QueryableExtensions
{
    public static async Task<DataSourceResult> ToKendoResultAsync<TEntity, TResult>(
        this IQueryable<TEntity> queryable, Expression<Func<TEntity, TResult>> selector, DataSourceRequest request)
        where TEntity : class, IEntity where TResult : class, new()
    {
        return await queryable.Select(selector).ToDataSourceResultAsync(request);
    }

    public static async Task<FileStreamResult> ToExcelResultAsync<TEntity, TResult>(this IQueryable<TEntity> queryable,
        Expression<Func<TEntity, TResult>> selector, DataSourceRequest request, string? fileName = null)
        where TEntity : class where TResult : class, new()
    {
        if (string.IsNullOrEmpty(fileName))
            fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
        request.Page = 1;
        request.PageSize = int.MaxValue;
        var dataSourceResult = await queryable.Select(selector).ToDataSourceResultAsync(request);
        var list = dataSourceResult.Data as IEnumerable<TResult>;
        
       var (stream, filename) =  Excel.ExcelHelper.ConvertToExcel(list);
        return new FileStreamResult(stream,"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            FileDownloadName = $"{WebUtility.UrlEncode(filename ?? fileName)}.xlsx"
        };
    }


    /// <summary>
    /// Applies paging, sorting, filtering and grouping using the information from the DataSourceRequest object.
    /// If the collection is already paged, the method returns an empty resullt.
    /// </summary>
    /// <param name="dataTable">An instance of <see cref="T:System.Data.DataTable" />.</param>
    /// <param name="request">An instance of <see cref="T:Kendo.Mvc.UI.DataSourceRequest" />.</param>
    /// <returns>
    /// A <see cref="T:Kendo.Mvc.UI.DataSourceResult" /> object, which contains the processed data after
    /// paging, sorting, filtering and grouping are applied.
    /// </returns>
    public static KendoResult<TModel> ToKendoResult<TModel>(
        this DataTable dataTable,
        DataSourceRequest request)
    {
        var dataSourceResult = global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResult(dataTable, request);
        return dataSourceResult.GetKendoResult<TModel>();
    }

    /// <summary>
    /// Applies paging, sorting, filtering and grouping using the information from the DataSourceRequest object.
    /// If the collection is already paged, the method returns an empty resullt.
    /// </summary>
    /// <param name="dataTable">An instance of <see cref="T:System.Data.DataTable" />.</param>
    /// <param name="request">An instance of <see cref="T:Kendo.Mvc.UI.DataSourceRequest" />.</param>
    /// <returns>
    /// A Task of <see cref="T:Kendo.Mvc.UI.DataSourceResult" /> object, which contains the processed data after
    /// paging, sorting, filtering and grouping are applied.
    /// It can be called with the "await" keyword for asynchronous operation.
    /// </returns>
    public static async Task<KendoResult<TModel>> ToKendoResultAsync<TModel>(
        this DataTable dataTable,
        DataSourceRequest request)
    {
        var dataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResultAsync(dataTable, request);
        return dataSourceResult.GetKendoResult<TModel>();
    }

    public static async Task<DataSourceResult> ToKendoResultAsync<TModel>(
        this DataTable dataTable,
        DataSourceRequest request,
        CancellationToken cancellationToken)
    {
        var dataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResultAsync(dataTable, request,
                cancellationToken);
        return dataSourceResult.GetKendoResult<TModel>();
    }

    /// <summary>
    /// Applies paging, sorting, filtering and grouping using the information from the DataSourceRequest object.
    /// If the collection is already paged, the method returns an empty result.
    /// </summary>
    /// <param name="enumerable">An instance of <see cref="T:System.Collections.IEnumerable" />.</param>
    /// <param name="request">An instance of <see cref="T:Kendo.Mvc.UI.DataSourceRequest" />.</param>
    /// <returns>
    /// A <see cref="T:Kendo.Mvc.UI.DataSourceResult" /> object, which contains the processed data after
    /// paging, sorting, filtering and grouping are applied.
    /// </returns>
    public static KendoResult<TModel> ToKendoResult<TModel>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request)
    {
        var dataSourceResult = global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResult(enumerable, request);
        return dataSourceResult.GetKendoResult<TModel>();
    }

    /// <summary>
    /// Applies paging, sorting, filtering and grouping using the information from the DataSourceRequest object.
    /// If the collection is already paged, the method returns an empty result.
    /// </summary>
    /// <param name="enumerable">An instance of <see cref="T:System.Collections.IEnumerable" />.</param>
    /// <param name="request">An instance of <see cref="T:Kendo.Mvc.UI.DataSourceRequest" />.</param>
    /// <returns>
    /// A Task of <see cref="T:Kendo.Mvc.UI.DataSourceResult" /> object, which contains the processed data
    /// after paging, sorting, filtering and grouping are applied.
    /// It can be called with the "await" keyword for asynchronous operation.
    /// </returns>
    public static async Task<KendoResult<TModel>> ToKendoResultAsync<TModel>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request)
    {
        var dataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResultAsync(enumerable, request);
        return dataSourceResult.GetKendoResult<TModel>();
    }

    public static async Task<KendoResult<TModel>> ToKendoResultAsync<TModel>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        CancellationToken cancellation)
    {
        var dataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResultAsync(enumerable, request, cancellation);
        return dataSourceResult.GetKendoResult<TModel>();
    }

    public static KendoResult<TModel> ToKendoResult<TModel>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        ModelStateDictionary modelState)
    {
        var dataSourceResult =
            global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResult(enumerable, request, modelState);
        return dataSourceResult.GetKendoResult<TModel>();
    }

    public static async Task<KendoResult<TModel>> ToKendoResultAsync<TModel>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        ModelStateDictionary modelState)
    {
        var dataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResultAsync(enumerable, request,
                modelState);
        return dataSourceResult.GetKendoResult<TModel>();
    }

    public static async Task<KendoResult<TModel>> ToKendoResultAsync<TModel>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        ModelStateDictionary modelState,
        CancellationToken cancellation)
    {
        var dataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResultAsync(enumerable, request,
                modelState, cancellation);
        return dataSourceResult.GetKendoResult<TModel>();
    }

    /// <summary>
    /// Applies paging, sorting, filtering and grouping using the information from the DataSourceRequest object.
    /// If the collection is already paged, the method returns an empty result.
    /// </summary>
    /// <param name="queryable">An instance of <see cref="T:System.Linq.IQueryable" />.</param>
    /// <param name="request">An instance of <see cref="T:Kendo.Mvc.UI.DataSourceRequest" />.</param>
    /// <returns>
    /// A <see cref="T:Kendo.Mvc.UI.DataSourceResult" /> object, which contains the processed data after paging, sorting, filtering and grouping are applied.
    /// </returns>
    public static KendoResult<TModel> ToKendoResult<TModel>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request)
    {
        var dataSourceResult = global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResult(queryable, request);
        return dataSourceResult.GetKendoResult<TModel>();
    }

    /// <summary>
    /// Applies paging, sorting, filtering and grouping using the information from the DataSourceRequest object.
    /// If the collection is already paged, the method returns an empty result.
    /// </summary>
    /// <param name="queryable">An instance of <see cref="T:System.Linq.IQueryable" />.</param>
    /// <param name="request">An instance of <see cref="T:Kendo.Mvc.UI.DataSourceRequest" />.</param>
    /// <returns>
    /// A Task of <see cref="T:Kendo.Mvc.UI.DataSourceResult" /> object, which contains the processed data
    /// after paging, sorting, filtering and grouping are applied.
    /// It can be called with the "await" keyword for asynchronous operation.
    /// </returns>
    public static async Task<KendoResult<TModel>> ToKendoResultAsync<TModel>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request)
    {
        var dataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResultAsync(queryable, request);
        return dataSourceResult.GetKendoResult<TModel>();
    }

    public static async Task<KendoResult<TModel>> ToKendoResultAsync<TModel>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        CancellationToken cancellationToken)
    {
        var dataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResultAsync(queryable, request,
                cancellationToken);
        return dataSourceResult.GetKendoResult<TModel>();
    }

    public static KendoResult<TResult> ToKendoResult<TModel, TResult>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Func<TModel, TResult> selector)
    {
        var dataSourceResult =
            global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResult(enumerable, request, selector);
        return dataSourceResult.GetKendoResult<TResult>();
    }

    public static async Task<KendoResult<TResult>> ToKendoResultAsync<TModel, TResult>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Func<TModel, TResult> selector)
    {
        var dataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResultAsync(enumerable, request,
                selector);
        return dataSourceResult.GetKendoResult<TResult>();
    }

    public static async Task<KendoResult<TResult>> ToKendoResultAsync<TModel, TResult>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Func<TModel, TResult> selector,
        CancellationToken cancellationToken)
    {
        var dataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResultAsync(enumerable, request,
                selector, cancellationToken);
        return dataSourceResult.GetKendoResult<TResult>();
    }

    public static KendoResult<TResult> ToKendoResult<TModel, TResult>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        ModelStateDictionary modelState,
        Func<TModel, TResult> selector)
    {
        var dataSourceResult =
            global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResult(enumerable, request, modelState,
                selector);
        return dataSourceResult.GetKendoResult<TResult>();
    }

    public static async Task<KendoResult<TResult>> ToKendoResultAsync<TModel, TResult>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        ModelStateDictionary modelState,
        Func<TModel, TResult> selector)
    {
        var dataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResultAsync(enumerable, request,
                modelState, selector);
        return dataSourceResult.GetKendoResult<TResult>();
    }

    public static async Task<KendoResult<TResult>> ToKendoResultAsync<TModel, TResult>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        ModelStateDictionary modelState,
        Func<TModel, TResult> selector,
        CancellationToken cancellationToken)
    {
        var dataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResultAsync(enumerable, request,
                modelState, selector, cancellationToken);
        return dataSourceResult.GetKendoResult<TResult>();
    }

    public static KendoResult<TResult> ToKendoResult<TModel, TResult>(
        this IQueryable<TModel> enumerable,
        DataSourceRequest request,
        Func<TModel, TResult> selector)
    {
        var dataSourceResult =
            global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResult(enumerable, request, selector);
        return dataSourceResult.GetKendoResult<TResult>();
    }

    public static async Task<KendoResult<TResult>> ToKendoResultAsync<TModel, TResult>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Func<TModel, TResult> selector)
    {
        var dataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResultAsync(queryable, request,
                selector);
        return dataSourceResult.GetKendoResult<TResult>();
    }

    public static async Task<KendoResult<TResult>> ToKendoResultAsync<TModel, TResult>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Func<TModel, TResult> selector,
        CancellationToken cancellationToken)
    {
        var dataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResultAsync(queryable, request, selector,
                cancellationToken);
        return dataSourceResult.GetKendoResult<TResult>();
    }

    public static KendoResult<TResult> ToKendoResult<TModel, TResult>(
        this IQueryable<TModel> enumerable,
        DataSourceRequest request,
        ModelStateDictionary modelState,
        Func<TModel, TResult> selector)
    {
        var dataSourceResult =
            global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResult(enumerable, request, modelState,
                selector);
        return dataSourceResult.GetKendoResult<TResult>();
    }

    public static async Task<KendoResult<TResult>> ToKendoResultAsync<TModel, TResult>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        ModelStateDictionary modelState,
        Func<TModel, TResult> selector)
    {
        var dataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResultAsync(queryable, request,
                modelState, selector);
        return dataSourceResult.GetKendoResult<TResult>();
    }

    public static async Task<KendoResult<TResult>> ToKendoResultAsync<TModel, TResult>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        ModelStateDictionary modelState,
        Func<TModel, TResult> selector,
        CancellationToken cancellationToken)
    {
        var dataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResultAsync(queryable, request,
                modelState, selector, cancellationToken);
        return dataSourceResult.GetKendoResult<TResult>();
    }

    public static KendoResult<TResult> ToKendoResult<TResult>(
        this IQueryable queryable,
        DataSourceRequest request,
        ModelStateDictionary modelState)
    {
        var dataSourceResult =
            global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResult(queryable, request, modelState);
        return dataSourceResult.GetKendoResult<TResult>();
    }

    public static async Task<KendoResult<TResult>> ToKendoResultAsync<TResult>(
        this IQueryable queryable,
        DataSourceRequest request,
        ModelStateDictionary modelState)
    {
        var dataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResultAsync(queryable, request,
                modelState);
        return dataSourceResult.GetKendoResult<TResult>();
    }

    public static async Task<KendoResult<TResult>> ToKendoResultAsync<TResult>(
        this IQueryable queryable,
        DataSourceRequest request,
        ModelStateDictionary modelState,
        CancellationToken cancellationToken)
    {
        var dataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToDataSourceResultAsync(queryable, request,
                modelState, cancellationToken);
        return dataSourceResult.GetKendoResult<TResult>();
    }


    /// <summary>
    /// Sorts the elements of a sequence using the specified sort descriptors.
    /// </summary>
    /// <param name="source">A sequence of values to sort.</param>
    /// <param name="sortDescriptors">The sort descriptors used for sorting.</param>
    /// <returns>
    /// An <see cref="T:System.Linq.IQueryable" /> whose elements are sorted according to a <paramref name="sortDescriptors" />.
    /// </returns>
    public static IQueryable Sort(
        this IQueryable source,
        IEnumerable<SortDescriptor> sortDescriptors)
    {
        return global::Kendo.Mvc.Extensions.QueryableExtensions.Sort(source, sortDescriptors);
    }

    /// <summary>
    /// Pages through the elements of a sequence until the specified
    /// <paramref name="pageIndex" /> using <paramref name="pageSize" />.
    /// </summary>
    /// <param name="source">A sequence of values to page.</param>
    /// <param name="pageIndex">Index of the page.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <returns>
    /// An <see cref="T:System.Linq.IQueryable" /> whose elements are at the specified <paramref name="pageIndex" />.
    /// </returns>
    public static IQueryable Page(this IQueryable source, int pageIndex, int pageSize)
    {
        return global::Kendo.Mvc.Extensions.QueryableExtensions.Page(source, pageIndex, pageSize);
    }


    /// <summary>
    /// Calls <see cref="M:Kendo.Mvc.Extensions.QueryableExtensions.OrderBy(System.Linq.IQueryable,System.Linq.Expressions.LambdaExpression)" />
    /// or <see cref="M:Kendo.Mvc.Extensions.QueryableExtensions.OrderByDescending(System.Linq.IQueryable,System.Linq.Expressions.LambdaExpression)" /> depending on the <paramref name="sortDirection" />.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="keySelector">The key selector.</param>
    /// <param name="sortDirection">The sort direction.</param>
    /// <returns>
    /// An <see cref="T:System.Linq.IQueryable" /> whose elements are sorted according to a key.
    /// </returns>
    public static IQueryable OrderBy(
        this IQueryable source,
        LambdaExpression keySelector,
        ListSortDirection? sortDirection)
    {
        return global::Kendo.Mvc.Extensions.QueryableExtensions.OrderBy(source, keySelector, sortDirection);
    }

    /// <summary>
    /// Groups the elements of a sequence according to a specified <paramref name="groupDescriptors" />.
    /// </summary>
    /// <param name="source"> An <see cref="T:System.Linq.IQueryable" /> whose elements to group. </param>
    /// <param name="groupDescriptors">The group descriptors used for grouping.</param>
    /// <returns>
    /// An <see cref="T:System.Linq.IQueryable" /> with <see cref="T:Kendo.Mvc.Infrastructure.IGroup" /> items,
    /// whose elements contains a sequence of objects and a key.
    /// </returns>
    public static IQueryable GroupBy(
        this IQueryable source,
        IEnumerable<GroupDescriptor> groupDescriptors,
        bool includeItems)
    {
        return global::Kendo.Mvc.Extensions.QueryableExtensions.GroupBy(source, groupDescriptors, includeItems);
    }

    public static IQueryable GroupBy(
        this IQueryable source,
        IQueryable notPagedData,
        IEnumerable<GroupDescriptor> groupDescriptors,
        bool includeItems)
    {
        return global::Kendo.Mvc.Extensions.QueryableExtensions.GroupBy(source, notPagedData, groupDescriptors,
            includeItems);
    }

    /// <summary>
    /// Calculates the results of given aggregates functions on a sequence of elements.
    /// </summary>
    /// <param name="source"> An <see cref="T:System.Linq.IQueryable" /> whose elements will
    /// be used for aggregate calculation.</param>
    /// <param name="aggregateFunctions">The aggregate functions.</param>
    /// <returns>Collection of <see cref="T:Kendo.Mvc.Infrastructure.AggregateResult" />s calculated for each function.</returns>
    public static AggregateResultCollection Aggregate(
        this IQueryable source,
        IEnumerable<AggregateFunction> aggregateFunctions)
    {
        return global::Kendo.Mvc.Extensions.QueryableExtensions.Aggregate(source, aggregateFunctions);
    }


    /// <summary> Returns the element at a specified index in a sequence.</summary>
    /// <returns> The element at the specified position in <paramref name="source" />.</returns>
    /// <param name="source"> An <see cref="T:System.Linq.IQueryable" /> to return an element from.</param>
    /// <param name="index"> The zero-based index of the element to retrieve.</param>
    /// <exception cref="T:System.ArgumentNullException"> <paramref name="source" /> is null.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException"> <paramref name="index" /> is less than zero.</exception>
    public static object ElementAt(this IQueryable source, int index)
    {
        return global::Kendo.Mvc.Extensions.QueryableExtensions.ElementAt(source, index);
    }


    /// <summary>
    /// Applies sorting, filtering and grouping using the information from the DataSourceRequest object.
    /// If the collection is already paged, the method returns an empty result.
    /// </summary>
    /// <param name="enumerable">An instance of <see cref="T:System.Collections.IEnumerable" />.</param>
    /// <param name="request">An instance of <see cref="T:Kendo.Mvc.UI.DataSourceRequest" />.</param>
    /// <returns>
    /// A <see cref="T:Kendo.Mvc.UI.TreeDataSourceResult" /> object, which contains the processed data after sorting, filtering and grouping are applied.
    /// </returns>
    public static TreeKendoResult<TResult> ToTreeKendoResult<TResult>(
        this IEnumerable<TResult> enumerable,
        DataSourceRequest request)
    {
        var treeDataSourceResult =
            global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResult(enumerable, request);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    /// <summary>
    /// Applies sorting, filtering and grouping using the information from the DataSourceRequest object.
    /// If the collection is already paged, the method returns an empty result.
    /// </summary>
    /// <param name="enumerable">An instance of <see cref="T:System.Collections.IEnumerable" />.</param>
    /// <param name="request">An instance of <see cref="T:Kendo.Mvc.UI.DataSourceRequest" />.</param>
    /// <returns>
    /// A Task of <see cref="T:Kendo.Mvc.UI.TreeDataSourceResult" /> object, which contains the processed data
    /// after sorting, filtering and grouping are applied.
    /// It can be called with the "await" keyword for asynchronous operation.
    /// </returns>
    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TResult>(
        this IEnumerable<TResult> enumerable,
        DataSourceRequest request)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(enumerable, request);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TResult>(
        this IEnumerable<TResult> enumerable,
        DataSourceRequest request,
        CancellationToken cancellationToken)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(enumerable, request,
                cancellationToken);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static TreeKendoResult<TResult> ToTreeKendoResult<TResult>(
        this IEnumerable<TResult> enumerable,
        DataSourceRequest request,
        ModelStateDictionary modelState)
    {
        var treeDataSourceResult =
            global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResult(enumerable, request, modelState);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TResult>(
        this IEnumerable<TResult> enumerable,
        DataSourceRequest request,
        ModelStateDictionary modelState)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(enumerable, request,
                modelState);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TResult>(
        this IEnumerable<TResult> enumerable,
        DataSourceRequest request,
        ModelStateDictionary modelState,
        CancellationToken cancellationToken)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(enumerable, request,
                modelState, cancellationToken);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static TreeKendoResult<TResult> ToTreeKendoResult<TModel, TResult>(
        this IQueryable<TModel> enumerable,
        DataSourceRequest request,
        Func<TModel, TResult> selector)
    {
        var treeDataSourceResult =
            global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResult(enumerable, request, selector);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TModel, TResult>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Func<TModel, TResult> selector)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(queryable, request,
                selector);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TModel, TResult>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Func<TModel, TResult> selector,
        CancellationToken cancellationToken)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(queryable, request,
                selector, cancellationToken);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static TreeKendoResult<TResult> ToTreeKendoResult<TModel, TResult>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Func<TModel, TResult> selector)
    {
        var treeDataSourceResult =
            global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResult(enumerable, request, selector);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TModel, TResult>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Func<TModel, TResult> selector)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(enumerable, request,
                selector);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TModel, TResult>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Func<TModel, TResult> selector,
        CancellationToken cancellationToken)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(enumerable, request,
                selector, cancellationToken);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static TreeKendoResult<TModel> ToTreeKendoResult<TModel, T1, T2>(
        this IQueryable<TModel> enumerable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector)
    {
        var treeDataSourceResult =
            global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResult<TModel, T1, T2>(enumerable, request,
                idSelector, parentIDSelector);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static async Task<TreeKendoResult<TModel>> ToTreeKendoResultAsync<TModel, T1, T2>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync<TModel, T1, T2>(
                queryable, request, idSelector, parentIDSelector);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static async Task<TreeKendoResult<TModel>> ToTreeKendoResultAsync<TModel, T1, T2>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        CancellationToken cancellationToken)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync<TModel, T1, T2>(
                queryable, request, idSelector, parentIDSelector, cancellationToken);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static TreeKendoResult<TModel> ToTreeKendoResult<TModel, T1, T2>(
        this IQueryable<TModel> enumerable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector)
    {
        var treeDataSourceResult =
            global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResult<TModel, T1, T2>(enumerable, request,
                idSelector, parentIDSelector, rootSelector);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static async Task<TreeKendoResult<TModel>> ToTreeKendoResultAsync<TModel, T1, T2>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync<TModel, T1, T2>(
                queryable, request, idSelector, parentIDSelector, rootSelector);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static async Task<TreeKendoResult<TModel>> ToTreeKendoResultAsync<TModel, T1, T2>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector,
        CancellationToken cancellationToken)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync<TModel, T1, T2>(
                queryable, request, idSelector, parentIDSelector, rootSelector, cancellationToken);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static TreeKendoResult<TResult> ToTreeKendoResult<TModel, T1, T2, TResult>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector,
        Func<TModel, TResult> selector)
    {
        var treeDataSourceResult =
            global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResult<TModel, T1, T2, TResult>(queryable,
                request, idSelector, parentIDSelector, rootSelector, selector);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TModel, T1, T2, TResult>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector,
        Func<TModel, TResult> selector)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync<TModel, T1, T2, TResult>(
                queryable, request, idSelector, parentIDSelector, rootSelector, selector);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TModel, T1, T2, TResult>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector,
        Func<TModel, TResult> selector,
        CancellationToken cancellationToken)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync<TModel, T1, T2, TResult>(
                queryable, request, idSelector, parentIDSelector, rootSelector, selector, cancellationToken);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static TreeKendoResult<TModel> ToTreeKendoResult<TModel, T1, T2>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        ModelStateDictionary modelState)
    {
        var treeDataSourceResult =
            global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResult(queryable, request, idSelector,
                parentIDSelector, modelState);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static async Task<TreeKendoResult<TModel>> ToTreeKendoResultAsync<TModel, T1, T2>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        ModelStateDictionary modelState)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(queryable, request,
                idSelector, parentIDSelector, modelState);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static async Task<TreeKendoResult<TModel>> ToTreeKendoResultAsync<TModel, T1, T2>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        ModelStateDictionary modelState,
        CancellationToken cancellationToken)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(queryable, request,
                idSelector, parentIDSelector, modelState, cancellationToken);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static TreeKendoResult<TModel> ToTreeKendoResult<TModel, T1, T2>(
        this IQueryable<TModel> enumerable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector,
        ModelStateDictionary modelState)
    {
        var treeDataSourceResult = global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResult(enumerable,
            request, idSelector, parentIDSelector, rootSelector, modelState);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static async Task<TreeKendoResult<TModel>> ToTreeKendoResultAsync<TModel, T1, T2>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector,
        ModelStateDictionary modelState)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(queryable, request,
                idSelector, parentIDSelector, rootSelector, modelState);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static async Task<TreeKendoResult<TModel>> ToTreeKendoResultAsync<TModel, T1, T2>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector,
        ModelStateDictionary modelState,
        CancellationToken cancellationToken)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(queryable, request,
                idSelector, parentIDSelector, rootSelector, modelState, cancellationToken);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static TreeKendoResult<TResult> ToTreeKendoResult<TModel, T1, T2, TResult>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Func<TModel, TResult> selector)
    {
        var treeDataSourceResult =
            global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResult(queryable, request, idSelector,
                parentIDSelector, selector);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TModel, T1, T2, TResult>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Func<TModel, TResult> selector)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(queryable, request,
                idSelector, parentIDSelector, selector);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TModel, T1, T2, TResult>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Func<TModel, TResult> selector,
        CancellationToken cancellationToken)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(queryable, request,
                idSelector, parentIDSelector, selector, cancellationToken);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static TreeKendoResult<TResult> ToTreeKendoResult<TModel, T1, T2, TResult>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        ModelStateDictionary modelState,
        Func<TModel, TResult> selector)
    {
        var treeDataSourceResult =
            global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResult(queryable, request, idSelector,
                parentIDSelector, modelState, selector);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TModel, T1, T2, TResult>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        ModelStateDictionary modelState,
        Func<TModel, TResult> selector)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(queryable, request,
                idSelector, parentIDSelector, modelState, selector);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TModel, T1, T2, TResult>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        ModelStateDictionary modelState,
        Func<TModel, TResult> selector,
        CancellationToken cancellationToken)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(queryable, request,
                idSelector, parentIDSelector, modelState, selector, cancellationToken);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static TreeKendoResult<TResult> ToTreeKendoResult<TModel, T1, T2, TResult>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector,
        ModelStateDictionary modelState,
        Func<TModel, TResult> selector)
    {
        var treeDataSourceResult = global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResult(queryable,
            request, idSelector, parentIDSelector, rootSelector, modelState, selector);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TModel, T1, T2, TResult>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector,
        ModelStateDictionary modelState,
        Func<TModel, TResult> selector)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(queryable, request,
                idSelector, parentIDSelector, rootSelector, modelState, selector);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TModel, T1, T2, TResult>(
        this IQueryable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector,
        ModelStateDictionary modelState,
        Func<TModel, TResult> selector,
        CancellationToken cancellationToken)
    {
        var treeDataSourceResult = await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(
            queryable, request, idSelector, parentIDSelector, rootSelector, modelState, selector, cancellationToken);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static TreeKendoResult<TModel> ToTreeKendoResult<TModel, T1, T2>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector)
    {
        var treeDataSourceResult =
            global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResult(enumerable, request, idSelector,
                parentIDSelector);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static async Task<TreeKendoResult<TModel>> ToTreeKendoResultAsync<TModel, T1, T2>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(enumerable, request,
                idSelector, parentIDSelector);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static async Task<TreeKendoResult<TModel>> ToTreeKendoResultAsync<TModel, T1, T2>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        CancellationToken cancellationToken)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(enumerable, request,
                idSelector, parentIDSelector, cancellationToken);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static TreeKendoResult<TModel> ToTreeKendoResult<TModel, T1, T2>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector)
    {
        var treeDataSourceResult =
            global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResult(enumerable, request, idSelector,
                parentIDSelector, rootSelector);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static async Task<TreeKendoResult<TModel>> ToTreeKendoResultAsync<TModel, T1, T2>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(enumerable, request,
                idSelector, parentIDSelector, rootSelector);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static async Task<TreeKendoResult<TModel>> ToTreeKendoResultAsync<TModel, T1, T2>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector,
        CancellationToken cancellationToken)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(enumerable, request,
                idSelector, parentIDSelector, rootSelector, cancellationToken);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static TreeKendoResult<TResult> ToTreeKendoResult<TModel, T1, T2, TResult>(
        this IEnumerable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector,
        Func<TModel, TResult> selector)
    {
        var treeDataSourceResult = global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResult(queryable,
            request, idSelector, parentIDSelector, rootSelector, selector);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TModel, T1, T2, TResult>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector,
        Func<TModel, TResult> selector)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(enumerable, request,
                idSelector, parentIDSelector, rootSelector, selector);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TModel, T1, T2, TResult>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector,
        Func<TModel, TResult> selector,
        CancellationToken cancellationToken)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(enumerable, request,
                idSelector, parentIDSelector, rootSelector, selector, cancellationToken);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static TreeKendoResult<TModel> ToTreeKendoResult<TModel, T1, T2>(
        this IEnumerable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        ModelStateDictionary modelState)
    {
        var treeDataSourceResult =
            global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResult(queryable, request, idSelector,
                parentIDSelector, modelState);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static async Task<TreeKendoResult<TModel>> ToTreeKendoResultAsync<TModel, T1, T2>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        ModelStateDictionary modelState)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(enumerable, request,
                idSelector, parentIDSelector, modelState);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static async Task<TreeKendoResult<TModel>> ToTreeKendoResultAsync<TModel, T1, T2>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        ModelStateDictionary modelState,
        CancellationToken cancellationToken)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(enumerable, request,
                idSelector, parentIDSelector, modelState, cancellationToken);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static TreeKendoResult<TModel> ToTreeKendoResult<TModel, T1, T2>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector,
        ModelStateDictionary modelState)
    {
        var treeDataSourceResult = global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResult(enumerable,
            request, idSelector, parentIDSelector, rootSelector, modelState);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static async Task<TreeKendoResult<TModel>> ToTreeKendoResultAsync<TModel, T1, T2>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector,
        ModelStateDictionary modelState)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(enumerable, request,
                idSelector, parentIDSelector, rootSelector, modelState);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static async Task<TreeKendoResult<TModel>> ToTreeKendoResultAsync<TModel, T1, T2>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector,
        ModelStateDictionary modelState,
        CancellationToken cancellationToken)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(enumerable, request,
                idSelector, parentIDSelector, rootSelector, modelState, cancellationToken);
        return treeDataSourceResult.GetTreeKendoResult<TModel>();
    }

    public static TreeKendoResult<TResult> ToTreeKendoResult<TModel, T1, T2, TResult>(
        this IEnumerable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Func<TModel, TResult> selector)
    {
        var treeDataSourceResult =
            global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResult(queryable, request, idSelector,
                parentIDSelector, selector);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TModel, T1, T2, TResult>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Func<TModel, TResult> selector)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(enumerable, request,
                idSelector, parentIDSelector, selector);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TModel, T1, T2, TResult>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Func<TModel, TResult> selector,
        CancellationToken cancellationToken)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(enumerable, request,
                idSelector, parentIDSelector, selector, cancellationToken);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static TreeKendoResult<TResult> ToTreeKendoResult<TModel, T1, T2, TResult>(
        this IEnumerable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        ModelStateDictionary modelState,
        Func<TModel, TResult> selector)
    {
        var treeDataSourceResult =
            global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResult(queryable, request, idSelector,
                parentIDSelector, modelState, selector);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TModel, T1, T2, TResult>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        ModelStateDictionary modelState,
        Func<TModel, TResult> selector)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(enumerable, request,
                idSelector, parentIDSelector, modelState, selector);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TModel, T1, T2, TResult>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        ModelStateDictionary modelState,
        Func<TModel, TResult> selector,
        CancellationToken cancellationToken)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(enumerable, request,
                idSelector, parentIDSelector, modelState, selector, cancellationToken);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static TreeKendoResult<TResult> ToTreeKendoResult<TModel, T1, T2, TResult>(
        this IEnumerable<TModel> queryable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector,
        ModelStateDictionary modelState,
        Func<TModel, TResult> selector)
    {
        var treeDataSourceResult = global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResult(queryable,
            request, idSelector, parentIDSelector, rootSelector, modelState, selector);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TModel, T1, T2, TResult>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector,
        ModelStateDictionary modelState,
        Func<TModel, TResult> selector)
    {
        var treeDataSourceResult =
            await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(enumerable, request,
                idSelector, parentIDSelector, rootSelector, modelState, selector);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }

    public static async Task<TreeKendoResult<TResult>> ToTreeKendoResultAsync<TModel, T1, T2, TResult>(
        this IEnumerable<TModel> enumerable,
        DataSourceRequest request,
        Expression<Func<TModel, T1>> idSelector,
        Expression<Func<TModel, T2>> parentIDSelector,
        Expression<Func<TModel, bool>> rootSelector,
        ModelStateDictionary modelState,
        Func<TModel, TResult> selector,
        CancellationToken cancellationToken)
    {
        var treeDataSourceResult = await global::Kendo.Mvc.Extensions.QueryableExtensions.ToTreeDataSourceResultAsync(
            enumerable, request, idSelector, parentIDSelector, rootSelector, modelState, selector, cancellationToken);
        return treeDataSourceResult.GetTreeKendoResult<TResult>();
    }
    
    
}