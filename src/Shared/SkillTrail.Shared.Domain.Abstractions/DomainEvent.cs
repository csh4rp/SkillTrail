namespace SkillTrail.Shared.Domain.Abstractions;

public abstract class DomainEvent
{
    public string Id { get; protected set; }
    
    public DateTime CreatedAt { get; protected set; }
}