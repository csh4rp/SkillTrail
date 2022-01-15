namespace SkillTrail.Shared.Domain.Abstractions.Events;

public abstract class DomainEvent
{
    public string Id { get; protected set; }
    
    public DateTime CreatedAt { get; protected set; }

    protected DomainEvent(string id, DateTime createdAt)
    {
        Id = id;
        CreatedAt = createdAt;
    }
}