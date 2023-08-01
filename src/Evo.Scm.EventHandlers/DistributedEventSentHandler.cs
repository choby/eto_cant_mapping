using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore.DistributedEvents;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;

namespace Evo.Scm;

public class DistributedEventSentHandler : ILocalEventHandler<DistributedEventSent>, ITransientDependency
{
    private IRepository<OutgoingEventRecord, Guid> _repository;

    public DistributedEventSentHandler(IRepository<OutgoingEventRecord, Guid> repository)
    {
        _repository = repository;
    }

    public async Task HandleEventAsync(DistributedEventSent eventData)
    {
        
        // TODO: IMPLEMENT YOUR LOGIC...
    }
}