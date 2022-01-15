using System.Text.Json;

namespace SkillTrail.Shared.Api.ModelBinding;

internal sealed class JsonModelBinder : IModelBinder
{
    public ModelBindingResult Bind(ModelBindingContext bindingContext)
    {
        var requestData = bindingContext.HttpRequestData;
        if (!requestData.Headers.TryGetValues("ContentType", out var values))
        {
            return ModelBindingResult.Unsuccessful();
        }

        var isContentJson = values.Any(v => v.Contains("json"));
        if (!isContentJson)
        {
            return ModelBindingResult.Unsuccessful();
        }

        if (requestData.Body.Length == 0)
        {
            return ModelBindingResult.Unsuccessful();
        }

        var buffer = new byte[requestData.Body.Length];
        requestData.Body.Read(buffer, 0, buffer.Length);
        var model = JsonSerializer.Deserialize(buffer, bindingContext.ModelType);
        return ModelBindingResult.Successful(model!);
    }
}