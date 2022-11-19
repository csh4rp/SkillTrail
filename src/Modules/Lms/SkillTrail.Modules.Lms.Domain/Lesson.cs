using SkillTrail.Shared.Abstractions.Domain;

namespace SkillTrail.Modules.Lms.Domain;

public class Lesson : IEntity
{
    public Guid Id { get; }
    public Guid CourseId { get; }
}