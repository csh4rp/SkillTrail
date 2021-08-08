using System;

namespace SkillTrail.Shared.Domain.Abstract
{
    public abstract class DomainEvent
    {
        protected DomainEvent()
        {
            EventId = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
        
        public Guid EventId { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        
    }
}