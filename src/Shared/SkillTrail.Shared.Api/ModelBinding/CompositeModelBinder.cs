using Microsoft.Azure.Functions.Worker.Http;

namespace SkillTrail.Shared.Api.ModelBinding;

public class CompositeModelBinder
{
    private readonly QueryStringModelBinder _queryStringModelBinder = new();
    private readonly JsonModelBinder _jsonModelBinder = new();
    private readonly IServiceProvider _serviceCollection;

    public CompositeModelBinder(IServiceProvider serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public bool TryBind<T>(HttpRequestData requestData, out T? model)
    {
        var binder = _serviceCollection.GetService(typeof(IModelBinder<>).MakeGenericType(typeof(T)));
        if (binder is not null)
        {
            return ((dynamic)binder).TryBind(requestData, out model);
        }
        
        if (requestData.Body.Length > 0 && _jsonModelBinder.TryBind(requestData, out model))
        {
            return true;
        }
        
        if ( _queryStringModelBinder.TryBind(requestData, out model))
        {
            return true;
        }

        return false;
    }
}