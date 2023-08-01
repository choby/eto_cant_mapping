using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;

namespace Evo.Scm.Auditing
{
    public interface IModificationAuditedObjectWithUserName: IModificationAuditedObject
    {
        string? LastModifierName { get; set; }
    }
}
