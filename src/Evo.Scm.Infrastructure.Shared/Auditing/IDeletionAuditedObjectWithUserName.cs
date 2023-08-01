using Volo.Abp.Auditing;

namespace Evo.Scm.Auditing
{
    public interface IDeletionAuditedObjectWithUserName: IDeletionAuditedObject
    {
        string? DeleterName { get; set; }
    }
}
