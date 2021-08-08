using System;
using SkillTrail.Shared.Domain.Abstract;
using SkillTrail.Shared.Domain.Utils;

namespace SkillTrail.Modules.Lms.Domain.Entities
{
    public abstract class CourseElement : AggregateRoot<int>
    {
        public int CourseId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? PublishedAt { get; private set; }
        public int Order { get; private set; }

        public void Publish() => PublishedAt = DateTimeProvider.Now;

    }
}