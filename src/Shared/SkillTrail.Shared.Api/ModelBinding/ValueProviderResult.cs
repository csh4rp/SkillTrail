namespace SkillTrail.Shared.Api.ModelBinding;

public record ValueProviderResult
{
    public static readonly ValueProviderResult Empty = new(null, false);

    private ValueProviderResult(object? value, bool isSuccessful)
    {
        Value = value;
        IsSuccessful = isSuccessful;
    }

    public object? Value { get; }
    
    public bool IsSuccessful { get; }

    public static ValueProviderResult Successful(object? value) => new(value, true);
}