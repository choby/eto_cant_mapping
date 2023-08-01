using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Uow;

namespace Evo.Scm.Domain;

public abstract class EventableEntity: Entity, IGeneratesDomainEvents
{
    private readonly ICollection<DomainEventRecord> _distributedEvents = new Collection<DomainEventRecord>();
    private readonly ICollection<DomainEventRecord> _localEvents = new Collection<DomainEventRecord>();

    public virtual IEnumerable<DomainEventRecord> GetLocalEvents()
    {
        return _localEvents;
    }

    public virtual IEnumerable<DomainEventRecord> GetDistributedEvents()
    {
        return _distributedEvents;
    }

    public virtual void ClearLocalEvents()
    {
        _localEvents.Clear();
    }

    public virtual void ClearDistributedEvents()
    {
        _distributedEvents.Clear();
    }
    /// <summary>
    /// 请优先使用AddLocalEvent发布事件
    /// </summary>
    /// <param name="eventData"></param>
    protected virtual void AddLocalEvent(object eventData)
    {
        _localEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }

    /// <summary>
    /// 优先使用本方法发布事件
    /// </summary>
    /// <param name="eventData"></param>
    protected virtual void AddDistributedEvent(object eventData)
    {
        _distributedEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }


}


public abstract class EventableEntity<TKey> : Entity<TKey>, IGeneratesDomainEvents
{
    private readonly ICollection<DomainEventRecord> _distributedEvents = new Collection<DomainEventRecord>();
    private readonly ICollection<DomainEventRecord> _localEvents = new Collection<DomainEventRecord>();

    public virtual IEnumerable<DomainEventRecord> GetLocalEvents()
    {
        return _localEvents;
    }

    public virtual IEnumerable<DomainEventRecord> GetDistributedEvents()
    {
        return _distributedEvents;
    }

    public virtual void ClearLocalEvents()
    {
        _localEvents.Clear();
    }

    public virtual void ClearDistributedEvents()
    {
        _distributedEvents.Clear();
    }
    /// <summary>
    /// 优先使用本方法发布事件
    /// </summary>
    /// <param name="eventData"></param>
    protected virtual void AddLocalEvent(object eventData)
    {
        _localEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }
    /// <summary>
    /// 请优先使用AddLocalEvent发布事件
    /// </summary>
    /// <param name="eventData"></param>
    protected virtual void AddDistributedEvent(object eventData)
    {
        _distributedEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }
}
