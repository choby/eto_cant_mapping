

namespace Evo.Scm.Positions;

/// <summary>
/// 用户下拉框Dto
/// </summary>
public class UserSelectOptionDto
{
    public UserSelectOptionDto(Guid id, string name)
    {
        this.Id = id;
        this.Name = name;
    }

    public Guid Id { get; set; }

    public string Name { get; set; }
}
