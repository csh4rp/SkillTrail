using System.Runtime.CompilerServices;
using Microsoft.Azure.Functions.Worker.Http;
using SkillTrail.Shared.Api.ModelBinding;

[assembly: InternalsVisibleTo("SkillTrail.Tests.Shared.Api")]

namespace SkillTrail.Shared.Api;

public abstract class BaseHttpFunction
{
    protected BaseHttpFunction(IModelBinder modelBinder)
    {
        ModelBinder = modelBinder;
    }

    protected IModelBinder ModelBinder { get; }

    protected bool TryBindModel<T>(HttpRequestData requestData, out T? model, BindingDataSource? bindingDataSource = null)
    {
        var bindingContext = new ModelBindingContext(requestData, typeof(T), bindingDataSource);
        var result = ModelBinder.Bind(bindingContext);
        if (result.IsSuccessful)
        {
            model = (T)result.Model!;
            return true;
        }

        model = default;
        return false;
    }
}