using Volo.Abp.Application.Services;


namespace Evo.Scm.Options
{
    public interface IOptionService : IApplicationService
    {
        Task<List<OptionItemDto>> GetItemListByCodeAsync(string code, bool? forFiltertion, bool isAll = false);
       
    }
}
