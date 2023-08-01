
namespace Evo.Scm.Suppliers;


public class SupplierSelectOptionDto
{
    public SupplierSelectOptionDto(Guid id, string sn,string shortName,string fullName,string mobile,string address)
    {
        this.Id = id;
        this.Sn = sn;
        this.ShortName = shortName;
        this.FullName = fullName;
        this.Mobile = mobile;
        this.Address = address;
    }

    /// <summary>
    /// 供应商Id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// 供应商编号
    /// </summary>
    public string Sn { get; set; }
    /// <summary>
    /// 简称
    /// </summary>
    public string ShortName { get; set; }
    /// <summary>
    /// 全称
    /// </summary>
    public string FullName { get; set; }
    /// <summary>
    /// 手机
    /// </summary>
    public string Mobile { get; set; }
    /// <summary>
    /// 详细地址
    /// </summary>
    public string Address { get; set; }
}
