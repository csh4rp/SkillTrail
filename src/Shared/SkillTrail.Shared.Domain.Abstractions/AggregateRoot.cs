using SkillTrail.Shared.Domain.Abstractions.Events;

namespace SkillTrail.Shared.Domain.Abstractions;

public abstract class AggregateRoot : Entity
{
    private HashSet<DomainEvent>? _domainEvents;

    public abstract string PartitionKey { get; protected set; }

    protected AggregateRoot(string id)
    {
        Id = id;
    }
    
    protected void RegisterEvent(DomainEvent domainEvent)
    {
        _domainEvents ??= new HashSet<DomainEvent>();
        _domainEvents.Add(domainEvent);
    }

    public IEnumerable<DomainEvent> GetEvents() => _domainEvents ?? Enumerable.Empty<DomainEvent>();

    public void ClearEvents() => _domainEvents?.Clear();
}
