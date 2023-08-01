using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Evo.Scm.Suppliers;


public class SupplierService : ScmAppService
{
    private readonly IRepository<Supplier, Guid> _repository;
    private readonly SupplierManager _supplierManager;

    public SupplierService(INoTrackingRepository<Supplier, Guid> noTrackingRepository, IRepository<Supplier, Guid> repository, SupplierManager supplierManager)
    {
       
        _repository = repository;
        _supplierManager = supplierManager;
        
    }


    /// <summary>
    /// 创建供应商
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<bool> CreateAsync(CreateSupplierDto input)
    {
        var supplier = new Supplier();
        supplier.Merchandiser = input.Merchandiser;
        supplier.Position = input.Position;
        supplier.Remark = input.Remark;
        supplier.Capacity = input.Capacity;
        supplier.Source = "自主添加";
        supplier.RepairDays = input.RepairDays;
        await _supplierManager.ChangeContactAsync(supplier, input.Contact);
        await _supplierManager.ChangeShortNameAsync(supplier, input.ShortName);
        await _supplierManager.ChangeFullNameAsync(supplier, input.FullName);
        await _supplierManager.ChangeMobileAsync(supplier, input.Mobile);
        await _supplierManager.ChangeRegionAsync(supplier, input.Province, input.City, input.District);
        await _supplierManager.ChangeAddressAsync(supplier, input.Address);
        await _supplierManager.ChangeIsEnabledAsync(supplier, input.IsEnabled);
        await _supplierManager.ChangeServiceClassAsync(supplier, input.ServiceClass);
        await _supplierManager.ChangeLevelAsync(supplier, input.Level);
        await _supplierManager.ChangeProductionClassAsync(supplier, input.ProductionClass);
        await _supplierManager.ChangeSpecializeInAsync(supplier, input.SpecializeIn);
        await _supplierManager.ChangeEvaluationScoreAsync(supplier, input.EvaluationScore);
        await _supplierManager.ChangeSnAsync(supplier, input.Sn);
        supplier.SetCode(input.Code);
        await _supplierManager.ChangeEnglishCode(supplier, input.EnglishCode);
        await _supplierManager.ChangeContract(supplier, input.ProductPurchaseContractSerial, input.ProductPurchaseContractTimeDate);
        supplier.SetAllowLogin(input.AllowLogin, input.RoleIds);
        await _repository.InsertAsync(supplier);        
        return true;       
    }

    /// <summary>
    /// 编辑供应商信息
    /// </summary>
    /// <param name="id">供应商Id</param>
    /// <param name="input">更新信息</param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(Guid id, UpdateSupplierDto input)
    {
        var supplier = await _repository.GetAsync(id, false);
        supplier.Merchandiser = input.Merchandiser;
        supplier.Position = input.Position;
        supplier.Remark = input.Remark;
        supplier.Capacity = input.Capacity;
        supplier.RepairDays = input.RepairDays;
        await _supplierManager.ChangeContactAsync(supplier, input.Contact);
        await _supplierManager.ChangeShortNameAsync(supplier, input.ShortName);
        await _supplierManager.ChangeFullNameAsync(supplier, input.FullName);
        await _supplierManager.ChangeMobileAsync(supplier, input.Mobile);
        await _supplierManager.ChangeRegionAsync(supplier, input.Province, input.City, input.District);
        await _supplierManager.ChangeAddressAsync(supplier, input.Address);
        await _supplierManager.ChangeIsEnabledAsync(supplier, input.IsEnabled);
        await _supplierManager.ChangeServiceClassAsync(supplier, input.ServiceClass);
        await _supplierManager.ChangeLevelAsync(supplier, input.Level);
        await _supplierManager.ChangeProductionClassAsync(supplier, input.ProductionClass);
        await _supplierManager.ChangeSpecializeInAsync(supplier, input.SpecializeIn);
        await _supplierManager.ChangeEvaluationScoreAsync(supplier, input.EvaluationScore);
        await _supplierManager.ChangeSnAsync(supplier, input.Sn);
        supplier.SetCode(input.Code);
        await _supplierManager.ChangeEnglishCode(supplier,input.EnglishCode);
        await _supplierManager.ChangeContract(supplier, input.ProductPurchaseContractSerial, input.ProductPurchaseContractTimeDate);
        //supplier.SetAllowLogin(input.AllowLogin, input.RoleIds);
        await _repository.UpdateAsync(supplier);
        return true;
    }
    

}
