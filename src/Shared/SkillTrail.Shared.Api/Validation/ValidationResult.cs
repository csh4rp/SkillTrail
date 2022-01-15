namespace SkillTrail.Shared.Api.Validation;

public class ValidationResult
{
    private static readonly ValidationResult ValidInstance = new(Array.Empty<ValidationError>());

    private ValidationResult(IReadOnlyCollection<ValidationError> validationErrors)
    {
        IsValid = validationErrors.Any();
        ValidationErrors = validationErrors;
    }
    
    public bool IsValid { get; }
    
    public IReadOnlyCollection<ValidationError> ValidationErrors { get; }

    public static ValidationResult Valid() => ValidInstance;

    public static ValidationResult Invalid(IReadOnlyCollection<ValidationError> validationErrors) =>
        new(validationErrors);
}