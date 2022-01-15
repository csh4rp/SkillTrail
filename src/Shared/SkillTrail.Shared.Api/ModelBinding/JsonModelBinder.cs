using System.Text.Json;
using Microsoft.Azure.Functions.Worker.Http;

namespace SkillTrail.Shared.Api.ModelBinding;

public class JsonModelBinder
{
    public bool TryBind<T>(HttpRequestData requestData, out T? model)
    {
        if (!requestData.Headers.TryGetValues("ContentType", out var values))
        {
            model = default;
            return false;
        }

        var isContentJson = values.Any(v => v.Contains("json"));
        if (!isContentJson)
        {
            model = default;
            return false;
        }

        if (requestData.Body.Length == 0)
        {
            model = default;
            return false;
        }

        var buffer = new byte[requestData.Body.Length];
        requestData.Body.Read(buffer, 0, buffer.Length);
        model = JsonSerializer.Deserialize<T>(buffer);
        return true;
    }
}