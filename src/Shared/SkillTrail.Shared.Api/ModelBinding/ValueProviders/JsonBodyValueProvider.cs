using System.Text.Json;

namespace SkillTrail.Shared.Api.ModelBinding.ValueProviders;

internal sealed class JsonBodyValueProvider : IValueProvider
{
    private readonly JsonDocument _document;

    public JsonBodyValueProvider(IHttpRequest httpRequest)
    {
        if (httpRequest.Body.Length == 0)
            throw new ArgumentException(nameof(httpRequest.Body));
        
        _document = JsonDocument.Parse(httpRequest.Body);
    }
    
    public ValueProviderResult GetValue(string key)
    {
        
        if (!_document.RootElement.TryGetProperty(key, out var property))
        {
            return ValueProviderResult.Empty;
        }
        
        var value =  GetPropertyValue(property);
        return ValueProviderResult.Successful(value);
    }

    private static object? GetPropertyValue(JsonElement jsonElement)
    {
        return jsonElement.ValueKind switch
        {
            JsonValueKind.Array => jsonElement.EnumerateArray().ToList(),
            JsonValueKind.Null => null,
            JsonValueKind.False => false,
            JsonValueKind.True => true,
            JsonValueKind.Number => GetNumber(jsonElement),
            JsonValueKind.Object => jsonElement.EnumerateObject().ToList(),
            JsonValueKind.String => GetString(jsonElement),
            JsonValueKind.Undefined => null,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static object GetNumber(JsonElement jsonElement)
    {
        if (jsonElement.TryGetInt32(out var intValue))
        {
            return intValue;
        }

        if (jsonElement.TryGetInt64(out var longValue))
        {
            return longValue;
        }

        if (jsonElement.TryGetDecimal(out var decimalValue))
        {
            return decimalValue;
        }

        if (jsonElement.TryGetDouble(out var doubleValue))
        {
            return doubleValue;
        }

        return 0;
    }

    private static object GetString(JsonElement jsonElement)
    {
        if (jsonElement.TryGetGuid(out var guidValue))
        {
            return guidValue;
        }

        if (jsonElement.TryGetDateTimeOffset(out var dateTimeOffsetValue))
        {
            return dateTimeOffsetValue;
        }

        if (jsonElement.TryGetDateTime(out var dateTimeValue))
        {
            return dateTimeValue;
        }

        return jsonElement.GetRawText();
    }
}