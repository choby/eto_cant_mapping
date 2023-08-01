
namespace Evo.Scm.DesignStyles;

public interface IDesignStyleService
{
    Task<IEnumerable<DesignStyleSelectOptionDto>> GetDesignStyleSelectOptionDataAsync(string sn,       bool? filterProduct);
}
