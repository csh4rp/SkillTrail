using System.Collections.Generic;

namespace SkillTrail.Tests.Shared.Api.ModelBinding.Models;

internal class DefaultCtorModel
{
    public int CourseId { get; set; }
    
    public int ModuleId { get; set; }
    
    public IEnumerable<int> UserIds { get; set; }
}