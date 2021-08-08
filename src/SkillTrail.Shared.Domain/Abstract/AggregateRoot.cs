using System.Collections.Generic;

namespace SkillTrail.Shared.Domain.Abstract
{
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot
    {
        private readonly HashSet<DomainEvent> _events = new();

        public IReadOnlyCollection<DomainEvent> DomainEvents => _events;

        protected virtual void RegisterEvent(DomainEvent domainEvent)
        {
            _events.Add(domainEvent);
        }

        public void ClearDomainEvents() => _events.Clear();
    }
}