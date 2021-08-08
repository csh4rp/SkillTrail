using System.Collections.Generic;

namespace SkillTrail.Shared.Domain.Abstract
{
    public interface IAggregateRoot
    {
        IReadOnlyCollection<DomainEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }
}