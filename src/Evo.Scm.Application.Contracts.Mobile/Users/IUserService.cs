
namespace Evo.Scm.Users;

public interface IUserService
{
    Task<ProfileDto> GetProfileAsync();
}