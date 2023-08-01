using EFCore.BulkExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Evo.Scm.Auditing;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IAuditingStore))]
public class AuditingStore : Volo.Abp.AuditLogging.AuditingStore, IAuditingStore, ITransientDependency
{
    IServiceProvider serviceProvider;
    public AuditingStore(
       IAuditLogRepository auditLogRepository,
       IUnitOfWorkManager unitOfWorkManager,
       IOptions<AbpAuditingOptions> options,
       IAuditLogInfoToAuditLogConverter converter,
       IServiceProvider serviceProvider) : base(auditLogRepository, unitOfWorkManager, options, converter)
    {
        this.serviceProvider = serviceProvider;
    }

    protected override Task SaveLogAsync(AuditLogInfo auditInfo)
    {
        return Task.Factory.StartNew(async () =>
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                var auditLogRepository = scope.ServiceProvider.GetRequiredService<IAuditLogRepository>();
                var converter = scope.ServiceProvider.GetRequiredService<IAuditLogInfoToAuditLogConverter>();
                using (var uow = unitOfWorkManager.Begin(true))
                {
                    var context = await auditLogRepository.GetDbContextAsync();
                    var auditlog = await converter.ConvertAsync(auditInfo);
                    await context.BulkInsertAsync(new List<AuditLog> { auditlog });
                    await context.BulkInsertAsync(auditlog.Actions.ToList());
                    await context.BulkInsertAsync(auditlog.EntityChanges.ToList());
                    foreach (var entityChange in auditlog.EntityChanges)
                    {
                        await context.BulkInsertAsync(entityChange.PropertyChanges.ToList());
                    }
                    //await AuditLogRepository.InsertAsync(await Converter.ConvertAsync(auditInfo));
                    //await context.BulkSaveChangesAsync();
                    await uow.CompleteAsync();
                }
            }
        });
    }
}
