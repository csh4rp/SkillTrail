namespace SkillTrail.Shared.Api.ModelBinding;

public record ModelValidationError(string PropertyName, string ErrorMessage);