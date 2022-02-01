namespace SkillTrail.Shared.Api.ModelBinding;

public interface IHttpRequest
{
    Uri Path { get; }
    
    Stream Body { get; }
    
    string Method { get; }
}