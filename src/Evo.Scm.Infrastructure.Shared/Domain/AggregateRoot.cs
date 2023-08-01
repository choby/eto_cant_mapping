using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;

namespace Evo.Scm.Domain
{
    [Serializable]
    public abstract class AggregateRoot : BasicAggregateRoot, IHasConcurrencyStamp
    {
        [DisableAuditing]
        public virtual string ConcurrencyStamp { get; set; }

        protected AggregateRoot()
        {
            ConcurrencyStamp = Guid.NewGuid().ToString("N");

        }
    }

    [Serializable]
    public abstract class AggregateRoot<TKey> : BasicAggregateRoot<TKey>,
      IHasConcurrencyStamp
    {

        [DisableAuditing]
        public virtual string ConcurrencyStamp { get; set; }

        protected AggregateRoot()
        {
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }

        protected AggregateRoot(TKey id)
            : base(id)
        {
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }
    }
}
