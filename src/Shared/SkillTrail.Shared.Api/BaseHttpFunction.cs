using Microsoft.Azure.Functions.Worker.Http;
using SkillTrail.Shared.Api.ModelBinding;

namespace SkillTrail.Shared.Api;

public abstract class BaseHttpFunction
{
    protected BaseHttpFunction(CompositeModelBinder modelBinder)
    {
        ModelBinder = modelBinder;
    }

    protected CompositeModelBinder ModelBinder { get; }

    protected bool TryBindModel<T>(HttpRequestData requestData, out T? model)
        => ModelBinder.TryBind<T>(requestData, out model);
}