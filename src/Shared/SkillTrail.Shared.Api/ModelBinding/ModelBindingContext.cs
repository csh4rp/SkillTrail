using Microsoft.Azure.Functions.Worker.Http;

namespace SkillTrail.Shared.Api.ModelBinding;

public class ModelBindingContext
{
    public ModelBindingContext(HttpRequestData httpRequestData, Type modelType, BindingDataSource? bindingDataSource)
    {
        HttpRequestData = httpRequestData;
        ModelType = modelType;
        BindingDataSource = bindingDataSource;
    }

    public HttpRequestData HttpRequestData { get; }
    
    public Type ModelType { get; }
    
    public BindingDataSource? BindingDataSource { get; }
    
}