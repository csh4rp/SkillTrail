using System.Collections.Generic;
using SkillTrail.Shared.Domain.Abstract;

namespace SkillTrail.Shared.Infrastructure.Database
{
    public interface IDomainEventSnapshotFactory
    {
        IReadOnlyCollection<DomainEventSnapshot> Create(IAggregateRoot aggregateRoot);
    }
}