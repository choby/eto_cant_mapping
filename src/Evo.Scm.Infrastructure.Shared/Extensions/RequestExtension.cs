using System.Data;
using System.Linq.Expressions;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.Infrastructure;
using Kendo.Mvc.Infrastructure.Implementation;
using Kendo.Mvc.Infrastructure.Implementation.Expressions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Evo.Scm.Kendo;

namespace Evo.Scm
{
    public static class RequestExtension
    {
        public static void SetDefaultCreationTimeSort(this DataSourceRequest request, ListSortDirection? direction)
        {
            if (request.Sorts == null || request.Sorts.Count == 0)
                request.Sorts = new List<SortDescriptor>() { new SortDescriptor("creationTime", direction ?? ListSortDirection.Descending) };
        }
        public static void SetDefaultFieldSort(this DataSourceRequest request, string fieldName, ListSortDirection? direction = null)
        {
            if (request.Sorts == null || request.Sorts.Count == 0)
                request.Sorts = new List<SortDescriptor>() { new SortDescriptor(fieldName, direction ?? ListSortDirection.Descending) };
            else
                request.Sorts.Add(new SortDescriptor(fieldName, direction ?? ListSortDirection.Descending));
        }
        
        public static void InsertFieldSort(this DataSourceRequest request,int index, string fieldName, ListSortDirection? direction = null)
        {
            if (request.Sorts != null && request.Sorts.Count > 0)
            {
                request.Sorts.Insert(index, new SortDescriptor(fieldName, direction ?? ListSortDirection.Descending));
            }

        }
    }
    


}
