using System;
using System.Collections.Generic;
using SkillTrail.Shared.Domain.Abstract;
using SkillTrail.Shared.Domain.ValueObjects;

namespace SkillTrail.Modules.Lms.Domain.Entities
{
    public class LearningPath : AggregateRoot<int>
    {
        private List<Course> _courses;

        public Language Language { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? PublishedAt { get; private set; }
    }
}