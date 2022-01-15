namespace SkillTrail.Shared.Domain.Abstractions.Events;

public interface IDomainEventHandler
{
    Task HandleAsync(DomainEvent domainEvent, CancellationToken cancellationToken);
}