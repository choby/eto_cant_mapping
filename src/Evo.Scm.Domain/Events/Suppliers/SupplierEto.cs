using System;
using AutoMapper;
using Evo.Scm.Suppliers;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;

namespace Evo.Scm.Domain;

[Serializable]
[EventName("Supplier")]
[AutoMap(typeof(Supplier))]
public class SupplierEto : EntityEto<Guid>
{
    public SupplierEto()
    {
    }

    /// <summary>
    /// 编号
    /// </summary>
    public string Sn { get;  set; }
    // /// <summary>
    // /// 联系人
    // /// </summary>
    // public string Contact { get;  set; }
    // /// <summary>
    // /// 职位
    // /// </summary>
    // public string Position { get; set; }
    // /// <summary>
    // /// 跟单员
    // /// </summary>
    // public string Merchandiser { get; set; }
    // /// <summary>
    // /// 简称
    // /// </summary>
    // public string ShortName { get;  set; }
    // /// <summary>
    // /// 全称
    // /// </summary>
    // public string FullName { get;  set; }
    // /// <summary>
    // /// 手机
    // /// </summary>
    // public string Mobile { get;  set; }
    // /// <summary>
    // /// 省
    // /// </summary>
    // public string Province { get;  set; }
    // /// <summary>
    // /// 市
    // /// </summary>
    // public string City { get;  set; }
    // /// <summary>
    // /// 区
    // /// </summary>
    // public string District { get;  set; }
    // /// <summary>
    // /// 详细地址
    // /// </summary>
    // public string Address { get;  set; }
    // /// <summary>
    // /// 开启状态
    // /// </summary>
    // public bool IsEnabled { get;  set; }
    // /// <summary>
    // /// 备注
    // /// </summary>
    // public string Remark { get; set; }
    // /// <summary>
    // /// 服务线
    // /// </summary>
    // public string ServiceClass { get;  set; }
    // /// <summary>
    // /// 评估得分
    // /// </summary>
    // public int? EvaluationScore { get;  set; }
    // /// <summary>
    // /// 来源
    // /// </summary>
    // public string Source { get; set; }
    // /// <summary>
    // /// 等级
    // /// </summary>
    // public string Level { get;  set; }
    // /// <summary>
    // /// 生产类别
    // /// </summary>
    // public string ProductionClass { get;  set; }
    // /// <summary>
    // /// 擅长模式
    // /// </summary>
    // public string SpecializeIn { get;  set; }
    // /// <summary>
    // /// 产能
    // /// </summary>
    // public int? Capacity { get; set; }
    // /// <summary>
    // /// 结算方式
    // /// </summary>
    // public string SettlementMode { get;  set; }
    // /// <summary>
    // /// 税务模式
    // /// </summary>
    // public string TaxMode { get;  set; }
    // /// <summary>
    // /// 是否开票
    // /// </summary>
    // public bool? HaveInvoice { get;  set; }
    // /// <summary>
    // /// 税率
    // /// </summary>
    // public decimal? TaxRate { get;  set; }
    // /// <summary>
    // /// 营业执照
    // /// </summary>
    // public string BusinessLicense { get; set; }
    // /// <summary>
    // /// 法人身份证
    // /// </summary>
    // public string LegalPersonIDCard { get; set; }    
    // /// <summary>
    // /// 租户Id
    // /// </summary>
    // public Guid? TenantId { get; set; }
    // /// <summary>
    // /// 代号
    // /// </summary>
    // public string Code { get;  set; }
    // /// <summary>
    // /// 合同编号
    // /// </summary>
    // public string ProductPurchaseContractSerial { get;  set; }
    // /// <summary>
    // /// 合同有效期
    // /// </summary>
    // public DateTime? ProductPurchaseContractTimeDate { get;  set; }
    // /// <summary>
    // /// 英文代号
    // /// </summary>
    // public string EnglishCode { get;  set; }
    // /// <summary>
    // /// 允许登录账号
    // /// </summary>
    // public bool? AllowLogin { get;  set; }
    //
    // /// <summary>
    // /// 返修天数
    // /// </summary>
    // public int? RepairDays { get;  set; }
}