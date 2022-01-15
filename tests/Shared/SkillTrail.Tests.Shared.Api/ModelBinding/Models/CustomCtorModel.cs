using System.Collections.Generic;

namespace SkillTrail.Tests.Shared.Api.ModelBinding.Models;

internal class CustomCtorModel
{
    public CustomCtorModel(int courseId, int moduleId, IEnumerable<int> userIds)
    {
        CourseId = courseId;
        ModuleId = moduleId;
        UserIds = userIds;
    }

    public int CourseId { get; }
    
    public int ModuleId { get; }
    
    public IEnumerable<int> UserIds { get; }
}