using System;
using System.Collections.Generic;
using SkillTrail.Shared.Domain.Abstract;

namespace SkillTrail.Modules.Lms.Domain.Entities
{
    public class LearningGroup : AggregateRoot<int>
    {
        private List<Student> _students;
        
        public string Name { get; private set; }
    }
}