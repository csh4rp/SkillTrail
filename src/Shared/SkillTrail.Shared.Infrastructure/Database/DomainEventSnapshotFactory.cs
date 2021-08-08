using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using SkillTrail.Shared.Domain.Abstract;

namespace SkillTrail.Shared.Infrastructure.Database
{
    internal sealed class DomainEventSnapshotFactory : IDomainEventSnapshotFactory
    {
        public IReadOnlyCollection<DomainEventSnapshot> Create(IAggregateRoot aggregateRoot)
        {
            if (!aggregateRoot.DomainEvents.Any())
            {
                return ImmutableArray<DomainEventSnapshot>.Empty;
            }
            
            var result = new List<DomainEventSnapshot>(aggregateRoot.DomainEvents.Count);
            var aggregateType = aggregateRoot.GetType();
            var aggregateId = ((dynamic) aggregateRoot).Id;
            var aggregateInfo = new
            {
                AggregateType = aggregateType.FullName,
                AggregateId = aggregateId
            };

            foreach (var domainEvent in aggregateRoot.DomainEvents)
            {
                var eventType = domainEvent.GetType();
                var eventInfo = new
                {
                    EventType = eventType.FullName,
                    EventData = domainEvent
                };

                var snapshot = new DomainEventSnapshot
                {
                    Id = domainEvent.EventId,
                    CreatedAt = domainEvent.CreatedAt,
                    AggregateData = JsonDocument.Parse(JsonSerializer.SerializeToUtf8Bytes(aggregateInfo)),
                    EventData = JsonDocument.Parse(JsonSerializer.SerializeToUtf8Bytes(eventInfo)),
                };
                
                result.Add(snapshot);
            }

            return result;
        }
    }
}