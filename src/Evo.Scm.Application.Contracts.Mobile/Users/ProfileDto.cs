using Evo.Scm.BrandIsolation;
using Evo.Scm.Departments;
using Evo.Scm.Permissions;
using Evo.User.Feishu;
using Volo.Abp.Application.Dtos;

namespace Evo.Scm.Users
{
    public class ProfileDto
    {
        public CacheBrandItem[] Brands { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string JobTitle { get; set; }
        public  IEnumerable<IEnumerable<IDepartment>> Departments { get; set; }
        public IEnumerable<IPermission> UserPermissions { get; set; }
    }

    public class UserPermissionsDto : EntityDto<Guid>
    {
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 受控路径
        /// </summary>
        public string Object { get; set; }
        /// <summary>
        /// 受控请求方式 GET/POST/PUT/DELETE/MENU/BUTTON
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 所属菜单
        /// </summary>
        public Guid? ParentId { get; set; }
    }
}
