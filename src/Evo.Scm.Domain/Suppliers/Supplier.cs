using Evo.Scm.Domain;
using Evo.Scm.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.MultiTenancy;
using Evo.Scm.Events;

namespace Evo.Scm.Suppliers;

public  class Supplier : FullAuditedAggregateRootWithUserName<Guid>, IMultiTenant
{
    public Supplier()
    {
        SupplierPaymentInfos =new List<SupplierPaymentInfo>();
    }
    /// <summary>
    /// 编号
    /// </summary>
    public virtual string Sn { get; private set; }
    /// <summary>
    /// 联系人
    /// </summary>
    public virtual string Contact { get; private set; }
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
    public string ShortName { get; private set; }
    /// <summary>
    /// 全称
    /// </summary>
    public string FullName { get; private set; }
    /// <summary>
    /// 手机
    /// </summary>
    public string Mobile { get; private set; }
    /// <summary>
    /// 省
    /// </summary>
    public string Province { get; private set; }
    /// <summary>
    /// 市
    /// </summary>
    public string City { get; private set; }
    /// <summary>
    /// 区
    /// </summary>
    public string District { get; private set; }
    /// <summary>
    /// 详细地址
    /// </summary>
    public string Address { get; private set; }
    /// <summary>
    /// 开启状态
    /// </summary>
    public bool IsEnabled { get; private set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }
    /// <summary>
    /// 服务线
    /// </summary>
    public string ServiceClass { get; private set; }
    /// <summary>
    /// 评估得分
    /// </summary>
    public int? EvaluationScore { get; private set; }
    /// <summary>
    /// 来源
    /// </summary>
    public string Source { get; set; }
    /// <summary>
    /// 等级
    /// </summary>
    public string Level { get; private set; }
    /// <summary>
    /// 生产类别
    /// </summary>
    public string ProductionClass { get; private set; }
    /// <summary>
    /// 擅长模式
    /// </summary>
    public string SpecializeIn { get; private set; }
    /// <summary>
    /// 产能
    /// </summary>
    public int? Capacity { get; set; }
    /// <summary>
    /// 结算方式
    /// </summary>
    public string SettlementMode { get; private set; }
    /// <summary>
    /// 税务模式
    /// </summary>
    public string TaxMode { get; private set; }
    /// <summary>
    /// 是否开票
    /// </summary>
    public bool? HaveInvoice { get;  set; }
    /// <summary>
    /// 税率
    /// </summary>
    public decimal? TaxRate { get; private set; }
    /// <summary>
    /// 营业执照
    /// </summary>
    public string BusinessLicense { get; set; }
    /// <summary>
    /// 法人身份证
    /// </summary>
    public string LegalPersonIDCard { get; set; }    
    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid? TenantId { get; set; }
    /// <summary>
    /// 代号
    /// </summary>
    public string Code { get; private set; }
    /// <summary>
    /// 合同编号
    /// </summary>
    public string ProductPurchaseContractSerial { get; private set; }
    /// <summary>
    /// 合同有效期
    /// </summary>
    public DateTime? ProductPurchaseContractTimeDate { get; private set; }
    /// <summary>
    /// 英文代号
    /// </summary>
    public string EnglishCode { get; private set; }
    /// <summary>
    /// 允许登录账号
    /// </summary>
    public bool? AllowLogin { get; private set; }

    /// <summary>
    /// 返修天数
    /// </summary>
    public int? RepairDays { get;  set; }

    /// <summary>
    /// 来源
    /// </summary>
    public virtual ICollection<SupplierPaymentInfo> SupplierPaymentInfos { get; private set; }

    /// <summary>
    /// 添加支付账户信息
    /// </summary>
    /// <param name="color"></param>
    internal void AddSupplierPaymentInfo(SupplierPaymentInfo info)
    {
        this.SupplierPaymentInfos.Add(info);
    }
    internal void RemoveSupplierPaymentInfo(Guid supplierPaymentId)
    {
        this.SupplierPaymentInfos.RemoveAll(x=>x.Id== supplierPaymentId);
    }
    /// <summary>
    /// 获取支付账户信息
    /// </summary>
    /// <param name="id"></param>
    internal SupplierPaymentInfo FindSupplierPaymentInfo(Guid id)
    {
        var supplierPaymentInfo = this.SupplierPaymentInfos.FirstOrDefault(x => x.Id == id);
        if (supplierPaymentInfo == null)
        {
            throw new BusinessException(ExceptionCodes.请求的数据未找到, $"找不到开户账户信息");
        }
        return supplierPaymentInfo;
    }

    internal void SetShortName(string shortName)
    {
        if (string.IsNullOrEmpty(shortName))
        {
            throw new BusinessException(ExceptionCodes.请求数据校验失败, $"请填写简称");
        }
        this.ShortName = shortName;
    }
    internal void SetContact(string contact)
    {
        if (string.IsNullOrEmpty(contact))
        {
            throw new BusinessException(ExceptionCodes.请求数据校验失败, $"请填写联系人");
        }
        this.Contact = contact;
    }
    internal void SetFullName(string fullName)
    {
        if (string.IsNullOrEmpty(fullName))
        {
            throw new BusinessException(ExceptionCodes.请求数据校验失败, $"请填写全称");
        }
        this.FullName = fullName;
    }
    internal void SetMobile(string mobile)
    {
        if (string.IsNullOrEmpty(mobile))
        {
            throw new BusinessException(ExceptionCodes.请求数据校验失败, $"请填写手机号码");
        }
        this.Mobile = mobile;
    }
    internal void SetRegion(string province, string city, string district)
    {
        if (string.IsNullOrEmpty(province))
        {
            throw new BusinessException(ExceptionCodes.请求数据校验失败, $"请选择地址省");
        }
        if (string.IsNullOrEmpty(city))
        {
            throw new BusinessException(ExceptionCodes.请求数据校验失败, $"请选择地址市");
        }
        if (string.IsNullOrEmpty(district))
        {
            throw new BusinessException(ExceptionCodes.请求数据校验失败, $"请选择地址区");
        }
        this.Province = province;
        this.City = city;
        this.District = district;
    }

    internal void SetAddress(string address)
    {
        if (string.IsNullOrEmpty(address))
        {
            throw new BusinessException(ExceptionCodes.请求数据校验失败, $"请填写详细地址");
        }
        this.Address = address;
    }
    internal void SetIsEnabled(bool isEnabled)
    {
        this.IsEnabled = isEnabled;
    }
    internal void SetServiceClass(string serviceClass)
    {
        if (string.IsNullOrEmpty(serviceClass))
        {
            throw new BusinessException(ExceptionCodes.请求数据校验失败, $"请选择服务线");
        }
        this.ServiceClass = serviceClass;
    }
    internal void SetLevel(string level)
    {
        if (string.IsNullOrEmpty(level))
        {
            throw new BusinessException(ExceptionCodes.请求数据校验失败, $"请选择等级");
        }
        this.Level = level;
    }
    internal void SetProductionClass(string productionClass)
    {
        if (string.IsNullOrEmpty(productionClass))
        {
            throw new BusinessException(ExceptionCodes.请求数据校验失败, $"请选择生产类别");
        }
        this.ProductionClass = productionClass;
    }
    internal void SetSpecializeIn(string specializeIn)
    {
        if (string.IsNullOrEmpty(specializeIn))
        {
            throw new BusinessException(ExceptionCodes.请求数据校验失败, $"请选择擅长模式");
        }
        this.SpecializeIn = specializeIn;
    }

    internal void SetSn(string sn)
    {
        Check.NotNullOrWhiteSpace(sn, nameof(sn));
        this.Sn = sn;
    } 
    public void SetSettlementMode(string settlementMode)
    {
        if (string.IsNullOrEmpty(settlementMode))
        {
            throw new BusinessException(ExceptionCodes.请求数据校验失败, $"请选择结算方式");
        }
        this.SettlementMode = settlementMode;
    }
    public void SetTaxMode(string taxMode)
    {
        if (string.IsNullOrEmpty(taxMode))
        {
            throw new BusinessException(ExceptionCodes.请求数据校验失败, $"请选择税务模式");
        }
        this.TaxMode = taxMode;
    }
    public void SetTaxRate(decimal? taxRate)
    {
        if (this.HaveInvoice.HasValue && this.HaveInvoice.Value)
        {
            if (!taxRate.HasValue)
            {
                throw new BusinessException(ExceptionCodes.请求数据校验失败, $"请填写税率");
            }
        }
        this.TaxRate = taxRate;
    }
    internal void SetEvaluationScore(int? evaluationScore)
    {
        this.EvaluationScore = evaluationScore;
    }

    public void SetCode(string code)
    {
        this.Code = code;
    }
    internal void SetEnglishCode(string englishCode)
    {
        this.EnglishCode = englishCode;
    }
    /// <summary>
    /// 设置合同
    /// </summary>
    /// <param name="serial">合同编号</param>
    /// <param name="timeDate">合同有效期</param>
    internal void SetContract(string serial,DateTime? timeDate)
    {
        this.ProductPurchaseContractSerial = serial;
        this.ProductPurchaseContractTimeDate = timeDate;
    }
    public void SetAllowLogin(bool? allowLogin, IEnumerable<Guid> roleIds)
    {
        this.AllowLogin = allowLogin;
        if (this.AllowLogin == true)
            //供应商启用事件
            AddLocalEvent(new SupplierEnabledEto(this, roleIds));
        else if(this.AllowLogin == false)
            AddLocalEvent(new SupplierDisabledEto(this));
    }
    
    /// <summary>
    /// 获取完整地址
    /// </summary>
    /// <returns></returns>
    public string GetFullAddress()
    {
        return $"{this.Province} {this.City} {this.District}{this.Address}";
    }
}