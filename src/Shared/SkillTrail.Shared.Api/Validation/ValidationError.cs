namespace SkillTrail.Shared.Api.Validation;

public class ValidationError
{
    public ValidationError(string propertyName, string message)
    {
        PropertyName = propertyName;
        Message = message;
    }

    public string PropertyName { get; }
    
    public string Message { get; }
}