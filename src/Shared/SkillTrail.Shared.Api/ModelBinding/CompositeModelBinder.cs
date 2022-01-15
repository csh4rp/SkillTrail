namespace SkillTrail.Shared.Api.ModelBinding;

internal sealed class CompositeModelBinder : IModelBinder
{
    private readonly QueryStringModelBinder _queryStringModelBinder = new();
    private readonly JsonModelBinder _jsonModelBinder = new();
    private readonly IModelBinderRegistry _modelBinderRegistry;
    
    public CompositeModelBinder(IModelBinderRegistry modelBinderRegistry)
    {
        _modelBinderRegistry = modelBinderRegistry;
    }

    public ModelBindingResult Bind(ModelBindingContext bindingContext)
    {
        if (_modelBinderRegistry.TryGetBinder(bindingContext.ModelType, out var binder) && binder is not null)
        {
            return binder.Bind(bindingContext);
        }

        if (ShouldTryBindFromBody(bindingContext))
        {
            return _jsonModelBinder.Bind(bindingContext);
        }

        return _queryStringModelBinder.Bind(bindingContext);
    }

    private static bool ShouldTryBindFromBody(ModelBindingContext bindingContext)
        => bindingContext.BindingDataSource == BindingDataSource.Body || (bindingContext.BindingDataSource is null &&
                                                                          bindingContext.HttpRequestData.Body.Length > 0);
    
}