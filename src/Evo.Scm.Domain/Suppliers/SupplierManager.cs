using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using System.Linq;
using Volo.Abp.Users;
using Evo.Scm.Sequences;
using Volo.Abp;
using Evo.Scm.ExceptionHandling;
using System.Collections.Generic;

namespace Evo.Scm.Suppliers;

public class SupplierManager : DomainService
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly ICurrentUser _currentUser;
   
    private readonly INoTrackingRepository<Supplier, Guid> _noTrackingRepository;
    public SupplierManager(IGuidGenerator guidGenerator, ICurrentUser currentUser, INoTrackingRepository<Supplier, Guid> noTrackingRepository)
    {
        _guidGenerator = guidGenerator;
        _currentUser = currentUser;
        
        _noTrackingRepository = noTrackingRepository;
    }
    /// <summary>
    /// 联系人
    /// </summary>
    /// <param name="supplier"></param>
    /// <param name="contact"></param>
    /// <returns></returns>
    public Task ChangeContactAsync(Supplier supplier, string contact)
    {
        supplier.SetContact(contact);
        return Task.CompletedTask;
    }
    /// <summary>
    /// 简称
    /// </summary>
    /// <param name="supplier"></param>
    /// <param name="shorName"></param>
    /// <returns></returns>
    public async Task ChangeShortNameAsync(Supplier supplier, string shortName)
    {
        var existing = await _noTrackingRepository.AnyAsync(x => x.ShortName == shortName && x.Id != supplier.Id);
        if (existing)
        {
            throw new BusinessException(ExceptionCodes.请求数据校验失败, $"简称{shortName}已存在");
        }
        supplier.SetShortName(shortName);
    }
    /// <summary>
    /// 全称
    /// </summary>
    /// <param name="supplier"></param>
    /// <param name="fullName"></param>
    /// <returns></returns>
    public async Task ChangeFullNameAsync(Supplier supplier, string fullName)
    {
        var existing = await _noTrackingRepository.AnyAsync(x => x.FullName == fullName && x.Id != supplier.Id);
        if (existing)
        {
            throw new BusinessException(ExceptionCodes.请求数据校验失败, $"全称{fullName}已存在");
        }
        supplier.SetFullName(fullName);
    }
    /// <summary>
    /// 手机
    /// </summary>
    /// <param name="supplier"></param>
    /// <param name="mobile"></param>
    /// <returns></returns>
    public Task ChangeMobileAsync(Supplier supplier, string mobile)
    {
        supplier.SetMobile(mobile);
        return Task.CompletedTask;
    }
    /// <summary>
    /// 省市区
    /// </summary>
    /// <param name="supplier"></param>
    /// <param name="province"></param>
    /// <param name="city"></param>
    /// <param name="district"></param>
    /// <returns></returns>
    public Task ChangeRegionAsync(Supplier supplier, string province, string city, string district)
    {
        supplier.SetRegion(province, city, district);
        return Task.CompletedTask;
    }
    /// <summary>
    /// 详情地址
    /// </summary>
    /// <param name="supplier"></param>
    /// <param name="address"></param>
    /// <returns></returns>
    public Task ChangeAddressAsync(Supplier supplier, string address)
    {
        supplier.SetAddress(address);
        return Task.CompletedTask;
    }
    /// <summary>
    /// 开启状态
    /// </summary>
    /// <param name="supplier"></param>
    /// <param name="isEnabled"></param>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    public Task ChangeIsEnabledAsync(Supplier supplier, bool isEnabled)
    {
        supplier.SetIsEnabled(isEnabled);
        return Task.CompletedTask;
    }
    /// <summary>
    /// 服务线
    /// </summary>
    /// <param name="supplier"></param>
    /// <param name="serviceClass"></param>
    /// <returns></returns>
    public Task ChangeServiceClassAsync(Supplier supplier, string serviceClass)
    {
        supplier.SetServiceClass(serviceClass);
        return Task.CompletedTask;
    }
    /// <summary>
    /// 等级
    /// </summary>
    /// <param name="supplier"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public Task ChangeLevelAsync(Supplier supplier, string level)
    {
        supplier.SetLevel(level);
        return Task.CompletedTask;
    }
    /// <summary>
    /// 生产类别
    /// </summary>
    /// <param name="supplier"></param>
    /// <param name="productionClass"></param>
    /// <returns></returns>
    public Task ChangeProductionClassAsync(Supplier supplier, string productionClass)
    {
        supplier.SetProductionClass(productionClass);
        return Task.CompletedTask;
    }
    /// <summary>
    /// 擅长模式
    /// </summary>
    /// <param name="supplier"></param>
    /// <param name="specializeIn"></param>
    /// <returns></returns>
    public Task ChangeSpecializeInAsync(Supplier supplier, string specializeIn)
    {
        if (supplier.ServiceClass == "成品线")
            supplier.SetSpecializeIn(specializeIn);
        return Task.CompletedTask;
    }
    public Task ChangeEvaluationScoreAsync(Supplier supplier, int? evaluationScore)
    {
        if (evaluationScore.HasValue)
        {
            if (evaluationScore.Value < 0)
                throw new BusinessException(ExceptionCodes.请求数据校验失败, $"产能不能少于0");
            if (evaluationScore.Value > 100)
                throw new BusinessException(ExceptionCodes.请求数据校验失败, $"产能不能大于100");
        }
        supplier.SetEvaluationScore(evaluationScore.Value);
        return Task.CompletedTask;
    }
    /// <summary>
    /// 序列号
    /// </summary>
    /// <param name="supplier"></param>
    /// <param name="sn"></param>
    /// <returns></returns>
    public async Task ChangeSnAsync(Supplier supplier, string sn)
    {
        if (string.IsNullOrEmpty(sn))
            sn = _guidGenerator.Create().ToString();
        supplier.SetSn(sn);
    }
    /// <summary>
    /// 添加支付账户
    /// </summary>
    /// <param name="supplier"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    public Task AddSupplierPaymentInfo(Supplier supplier, SupplierPaymentInfo info)
    {
        info.SetId(_guidGenerator.Create());
        info.SetSupplierId(supplier.Id);
        supplier.AddSupplierPaymentInfo(info);
        return Task.CompletedTask;
    }
    public Task RemoveSupplierPaymentInfo(Supplier supplier, Guid supplierPaymentInfoId)
    {
        supplier.RemoveSupplierPaymentInfo(supplierPaymentInfoId);
        return Task.CompletedTask;
    }
    /// <summary>
    /// 编辑支付账户
    /// </summary>
    /// <param name="supplier"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<SupplierPaymentInfo> FindSupplierPaymentInfo(Supplier supplier, Guid id)
    {
        var supplierPaymentInfo = supplier.FindSupplierPaymentInfo(id);
        return Task.FromResult(supplierPaymentInfo);
    }
    /// <summary>
    /// 支付账户设为默认
    /// </summary>
    /// <param name="supplier"></param>
    /// <param name="paymentId"></param>
    /// <returns></returns>
    public Task ChangeDefaultPayment(Supplier supplier, Guid paymentId)
    {
        supplier.SupplierPaymentInfos.FirstOrDefault(x => x.IsDefault)?.SetIsDefault(false);
        supplier.SupplierPaymentInfos.FirstOrDefault(x => x.Id == paymentId).SetIsDefault(true);
        return Task.CompletedTask;
    }
    public Task ChangePaymentApproved(SupplierPaymentInfo supplierPaymentInfo)
    {
        supplierPaymentInfo.SetIsEnabled(true);
        supplierPaymentInfo.ApproverName = _currentUser.Name;
        supplierPaymentInfo.ApproveTime = DateTime.Now;
        return Task.CompletedTask;
    }
    public Task ChangePaymentUnApproved(SupplierPaymentInfo supplierPaymentInfo)
    {
        supplierPaymentInfo.SetIsEnabled(false);
        supplierPaymentInfo.ApproverName = null;
        supplierPaymentInfo.ApproveTime = null;
        return Task.CompletedTask;
    }
    /// <summary>
    /// 擅长模式
    /// </summary>
    /// <param name="supplier"></param>
    /// <param name="specializeIn"></param>
    /// <returns></returns>
    public Task ChangeContract(Supplier supplier, string contractSerial, DateTime? contractTimeDate)
    {
        if (supplier.ServiceClass == "成品线")
            supplier.SetContract(contractSerial, contractTimeDate);
        return Task.CompletedTask;
    }
    /// <summary>
    /// 代号
    /// </summary>
    /// <param name="supplier"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    public async Task ChangeEnglishCode(Supplier supplier, string englishCode)
    {
        if (string.IsNullOrEmpty(englishCode))
            return;
        var any = await _noTrackingRepository.AnyAsync(x => x.Id != supplier.Id && x.EnglishCode == englishCode);
        if (any)
        {
            throw new BusinessException(ExceptionCodes.请求数据校验失败, $"代号{englishCode}已存在");
        }
        supplier.SetEnglishCode(englishCode);
    }
}
