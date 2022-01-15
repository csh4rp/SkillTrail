namespace SkillTrail.Tests.Shared.Api.ModelBinding.Models;

internal class PrivateCtorModel
{
    private PrivateCtorModel(int courseId)
    {
        CourseId = courseId;
    }
    
    public int CourseId { get; }
    
}