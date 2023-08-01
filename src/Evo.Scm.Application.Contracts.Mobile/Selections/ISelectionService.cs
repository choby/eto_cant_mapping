using Volo.Abp.Application.Services;

namespace Evo.Scm.Selections;

public interface ISelectionService : IApplicationService
{
    Task<IEnumerable<SelectionSelectOptionDto>> GetSelectionSelectOptionDataAsync(string sn);
}
