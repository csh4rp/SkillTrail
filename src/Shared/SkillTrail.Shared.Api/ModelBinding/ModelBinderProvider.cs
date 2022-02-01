using SkillTrail.Shared.Api.ModelBinding.ModelBinders;

namespace SkillTrail.Shared.Api.ModelBinding;

internal sealed class ModelBinderProvider : IModelBinderProvider
{
    private readonly IModelBinderRegistry _modelBinderRegistry;

    public ModelBinderProvider(IModelBinderRegistry modelBinderRegistry)
        => _modelBinderRegistry = modelBinderRegistry;
    
    public IModelBinder? GetBinder(ModelBindingContext modelBindingContext)
    {
        if (_modelBinderRegistry.TryGet(modelBindingContext.ModelType, out var modelBinder))
        {
            return modelBinder;
        }

        var request = modelBindingContext.HttpRequest;
        if (request.Method == "GET" && !string.IsNullOrEmpty(request.Path.Query))
        {
            return new QueryStringModelBinder();
        }

        if (request.Method != "GET" && request.Body.Length > 0 && request.Body.CanSeek)
        {
            return null;
        }
        
        return null;
    }
}