namespace SkillTrail.Shared.Api.ModelBinding;

public class ModelBindingResult
{
    private static readonly ModelBindingResult UnsuccessfulInstance = new(false, null);
    
    public bool IsSuccessful { get; }
        
    public object? Model { get; }
    
    private ModelBindingResult(bool isSuccessful, object? model)
    {
        IsSuccessful = isSuccessful;
        Model = model;
    }

    public static ModelBindingResult Successful(object model) => new(true, model);

    public static ModelBindingResult Unsuccessful() => UnsuccessfulInstance;
}