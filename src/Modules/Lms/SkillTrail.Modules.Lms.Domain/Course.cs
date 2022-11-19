using SkillTrail.Shared.Abstractions.Domain;

namespace SkillTrail.Modules.Lms.Domain;

public class Course : IEntity
{
    public Guid Id { get; }
    public Guid? CoursePathId { get; }
}