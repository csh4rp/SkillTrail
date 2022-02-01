namespace SkillTrail.Shared.Api.ModelBinding;

public class ModelBindingContext
{
    public Type ModelType { get; }
    
    public IHttpRequest HttpRequest { get; }
    
    public IValueProvider ValueProvider { get; }
    
}