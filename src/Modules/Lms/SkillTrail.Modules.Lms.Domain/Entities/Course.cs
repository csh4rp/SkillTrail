using System;
using SkillTrail.Shared.Domain.Abstract;
using SkillTrail.Shared.Domain.ValueObjects;

namespace SkillTrail.Modules.Lms.Domain.Entities
{
    public class Course : AggregateRoot<int>
    {
        public Language Language { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? PublishedAt { get; private set; }

    }
}