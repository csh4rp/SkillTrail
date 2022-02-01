namespace SkillTrail.Shared.Api.ModelBinding;

public sealed class ModelBindingResult
{
    private static readonly ModelBindingResult UnsuccessfulInstance = new(null, false);
    
    private ModelBindingResult(object? model, bool isSuccessful)
    {
        Model = model;
        IsSuccessful = isSuccessful;
    }

    public object? Model { get; }
    
    public bool IsSuccessful { get; }
    
    public static ModelBindingResult Successful(object model) 
        => new(model, true);

    public static ModelBindingResult Unsuccessful() => UnsuccessfulInstance;

}