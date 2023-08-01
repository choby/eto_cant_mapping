
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Evo.Scm.Suppliers;

public class UpdateSupplierDto:IValidatableObject
{
    /// <summary>
    /// 编号
    /// </summary>
    public string Sn { get; set; }
    /// <summary>
    /// 联系人
    /// </summary>
    [Required]
    public string Contact { get; set; }
    /// <summary>
    /// 职位
    /// </summary>
    public string Position { get; set; }
    /// <summary>
    /// 跟单员
    /// </summary>
    public string Merchandiser { get; set; }
    /// <summary>
    /// 简称
    /// </summary>
    [Required]
    public string ShortName { get; set; }
    /// <summary>
    /// 全称
    /// </summary>
    [Required]
    public string FullName { get; set; }
    /// <summary>
    /// 手机
    /// </summary>
    [Required]
    public string Mobile { get; set; }
    /// <summary>
    /// 省
    /// </summary>
    [Required]
    public string Province { get; set; }
    /// <summary>
    /// 市
    /// </summary>
    [Required]
    public string City { get; set; }
    /// <summary>
    /// 区
    /// </summary>
    [Required]
    public string District { get; set; }
    /// <summary>
    /// 详细地址
    /// </summary>
    [Required]
    public string Address { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    [Required]
    public bool IsEnabled { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }
    /// <summary>
    /// 服务线
    /// </summary>
    [Required]
    public string ServiceClass { get; set; }
    /// <summary>
    /// 等级
    /// </summary>
    [Required]
    public string Level { get; set; }
    /// <summary>
    /// 生成类别
    /// </summary>
    [Required]
    public string ProductionClass { get; set; }
    /// <summary>
    /// 擅长模式
    /// </summary>
    public string SpecializeIn { get; set; }
    /// <summary>
    /// 产能
    /// </summary>
    public int? Capacity { get; set; }
    /// <summary>
    /// 评估得分
    /// </summary>
    public int? EvaluationScore { get; set; }
    /// <summary>
    /// 代号
    /// </summary>
    public string Code { get; set; }
    /// <summary>
    /// 合同编号
    /// </summary>
    public string ProductPurchaseContractSerial { get; set; }
    /// <summary>
    /// 合同有效时间
    /// </summary>
    public DateTime? ProductPurchaseContractTimeDate { get; set; }
    /// <summary>
    /// 英文代号
    /// </summary>
    public string EnglishCode { get; set; }
    ///// <summary>
    ///// 允许登录EVO系统
    ///// </summary>
    //public bool? AllowLogin { get; set; }
    ///// <summary>
    ///// 角色Id集合
    ///// </summary>
    //public IEnumerable<Guid> RoleIds { get; set; }
    /// <summary>
    /// 返修天数
    /// </summary>
    public int? RepairDays { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(this.ServiceClass == "成品线" && !(this.RepairDays > 0))
            yield return new ValidationResult(
                "服务线为成品线时，返修天数为必填",
                new[] { "RepairDays"}
            );
    }
}
