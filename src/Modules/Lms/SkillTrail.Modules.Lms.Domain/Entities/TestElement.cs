using SkillTrail.Shared.Domain.Abstract;

namespace SkillTrail.Modules.Lms.Domain.Entities
{
    public class TestElement : Entity<int>
    {
        public int CourseElementId { get; private set; }
    }
}