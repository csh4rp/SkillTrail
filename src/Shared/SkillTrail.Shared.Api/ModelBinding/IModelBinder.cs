using Microsoft.Azure.Functions.Worker.Http;

namespace SkillTrail.Shared.Api.ModelBinding;

public interface IModelBinder<T>
{
    bool TryBindModel(HttpRequestData httpRequestData, out T? model);
}