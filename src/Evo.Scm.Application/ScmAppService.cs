using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Evo.Scm.Localization;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Evo.Scm;

[Authorize]
public abstract class ScmAppService : ApplicationService
{
    
    protected ScmAppService()
    {
        
    }
    
    /// <summary>
    /// Should apply sorting if needed.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="input">The input.</param>
    protected virtual IQueryable<TEntity> ApplySorting<TEntity,TGetListInput>(IQueryable<TEntity> query, TGetListInput input)
    {
        //Try to sort query if available
        if (input is ISortedResultRequest sortInput)
        {
            if (!sortInput.Sorting.IsNullOrWhiteSpace())
            {
                return query.OrderBy(sortInput.Sorting);
            }
        }

        //IQueryable.Task requires sorting, so we should sort if Take will be used.
        // if (input is ILimitedResultRequest)
        // {
        //     return query.OrderByDescending(e => e.Id);
        // }

        //No sorting
        return query;
    }
    
    /// <summary>
    /// Should apply paging if needed.
    /// </summary>
    /// <param name="query">The query.</param>
    /// <param name="input">The input.</param>
    protected virtual IQueryable<TEntity> ApplyPaging<TEntity,TGetListInput>(IQueryable<TEntity> query, TGetListInput input)
    {
        //Try to use paging if available
        if (input is IPagedResultRequest pagedInput)
        {
            return query.PageBy(pagedInput);
        }

        //Try to limit query result if available
        if (input is ILimitedResultRequest limitedInput)
        {
            return query.Take(limitedInput.MaxResultCount);
        }

        //No paging
        return query;
    }

    
    
}
