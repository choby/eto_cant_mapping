using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.Infrastructure;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Evo.Scm.ModelBinders.DataSourceRequestModelBinder;

public class DataSourceRequestModelBinder :IModelBinder
{
    public virtual Task BindModelAsync(ModelBindingContext bindingContext)
    {
      DataSourceRequest dataSourceRequest = DataSourceRequestModelBinder.CreateDataSourceRequest(bindingContext.ModelMetadata, bindingContext.ValueProvider, bindingContext.ModelName);
      bindingContext.Result = ModelBindingResult.Success((object) dataSourceRequest);
      return Task.CompletedTask;
    }

    private static void TryGetValue<T>(
      ModelMetadata modelMetadata,
      IValueProvider valueProvider,
      string modelName,
      string key,
      Action<T> action)
    {
      if (modelMetadata.BinderModelName.HasValue())
        key = modelName + "-" + key;
      ValueProviderResult result = valueProvider.GetValue(key);
      if (result.FirstValue == null)
        return;
      object obj = result.ConvertValueTo(typeof (T));
      if (obj == null)
        return;
      action((T) obj);
    }

    public static DataSourceRequest CreateDataSourceRequest(
      ModelMetadata modelMetadata,
      IValueProvider valueProvider,
      string modelName)
    {
      DataSourceRequest request = new DataSourceRequest();
      DataSourceRequestModelBinder.TryGetValue<string>(modelMetadata, valueProvider, modelName, DataSourceRequestUrlParameters.Sort, (Action<string>) (sort => request.Sorts = DataSourceDescriptorSerializer.Deserialize<SortDescriptor>(sort)));
      DataSourceRequestModelBinder.TryGetValue<int>(modelMetadata, valueProvider, modelName, DataSourceRequestUrlParameters.Page, (Action<int>) (currentPage => request.Page = currentPage));
      DataSourceRequestModelBinder.TryGetValue<int>(modelMetadata, valueProvider, modelName, DataSourceRequestUrlParameters.PageSize, (Action<int>) (pageSize => request.PageSize = pageSize));
      DataSourceRequestModelBinder.TryGetValue<bool>(modelMetadata, valueProvider, modelName, DataSourceRequestUrlParameters.GroupPaging, (Action<bool>) (groupPaging => request.GroupPaging = groupPaging));
      DataSourceRequestModelBinder.TryGetValue<bool>(modelMetadata, valueProvider, modelName, DataSourceRequestUrlParameters.IncludeSubGroupCount, (Action<bool>) (includeSubGroupCount => request.IncludeSubGroupCount = includeSubGroupCount));
      DataSourceRequestModelBinder.TryGetValue<int>(modelMetadata, valueProvider, modelName, DataSourceRequestUrlParameters.Skip, (Action<int>) (skip => request.Skip = skip));
      DataSourceRequestModelBinder.TryGetValue<int>(modelMetadata, valueProvider, modelName, DataSourceRequestUrlParameters.Take, (Action<int>) (take => request.Take = take));
      DataSourceRequestModelBinder.TryGetValue<string>(modelMetadata, valueProvider, modelName, DataSourceRequestUrlParameters.Filter, (Action<string>) (filter => request.Filters = FilterDescriptorFactory.Create(filter)));
      DataSourceRequestModelBinder.TryGetValue<string>(modelMetadata, valueProvider, modelName, DataSourceRequestUrlParameters.Group, (Action<string>) (group => request.Groups = DataSourceDescriptorSerializer.Deserialize<GroupDescriptor>(group)));
      DataSourceRequestModelBinder.TryGetValue<string>(modelMetadata, valueProvider, modelName, DataSourceRequestUrlParameters.Aggregates, (Action<string>) (aggregates => request.Aggregates = DataSourceDescriptorSerializer.Deserialize<AggregateDescriptor>(aggregates)));
      return request;
    }
}