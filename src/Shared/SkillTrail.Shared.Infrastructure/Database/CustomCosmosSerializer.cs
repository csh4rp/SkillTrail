using System.Text.Json;
using Microsoft.Azure.Cosmos;

namespace SkillTrail.Shared.Infrastructure.Database;

public sealed class CustomCosmosSerializer : CosmosSerializer
{
    private readonly JsonSerializerOptions _options;

    public CustomCosmosSerializer()
    {
        _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public override T FromStream<T>(Stream stream) => JsonSerializer.Deserialize<T>(stream, _options)!;
    
    public override Stream ToStream<T>(T input)
    {
        var stream = new MemoryStream();
        JsonSerializer.Serialize(stream, input, _options);
        return stream;
    }
}