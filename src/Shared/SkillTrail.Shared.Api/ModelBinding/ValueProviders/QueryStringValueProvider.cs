namespace SkillTrail.Shared.Api.ModelBinding.ValueProviders;

internal sealed class QueryStringValueProvider : IValueProvider
{
    private readonly Dictionary<string, string[]> _queryStringValues;

    public QueryStringValueProvider(IHttpRequest httpRequest)
    {
        if (string.IsNullOrEmpty(httpRequest.Path.Query))
            _queryStringValues = new Dictionary<string, string[]>();
        
        _queryStringValues = httpRequest.Path.Query.Substring(1).Split("&")
            .Select(s =>
            {
                var values = s.Split("=");
                return new KeyValuePair<string, string>(values[0].ToLower(), values[1]);
            })
            .GroupBy(k => k.Key)
            .ToDictionary(k => k.Key, v => v.Select(kv => kv.Value).ToArray());
    }

    public ValueProviderResult GetValue(string key)
        => _queryStringValues.TryGetValue(key.ToLower(), out var values)
            ? ValueProviderResult.Successful(values)
            : ValueProviderResult.Empty;
}