using Kendo.Mvc;
using Kendo.Mvc.Infrastructure.Implementation;

namespace Evo.Scm.ModelBinders.DataSourceRequestModelBinder;

public class FilterDescriptorFactory
{
    public static IList<IFilterDescriptor> Create(string input)
    {
        IList<IFilterDescriptor> filterDescriptorList = (IList<IFilterDescriptor>) new List<IFilterDescriptor>();
        IFilterNode filterNode = new FilterParser(input).Parse();
        if (filterNode == null)
            return filterDescriptorList;
        FilterNodeVisitor visitor = new FilterNodeVisitor();
        filterNode.Accept((IFilterNodeVisitor) visitor);
        filterDescriptorList.Add(visitor.Result);
        return filterDescriptorList;
    }
}