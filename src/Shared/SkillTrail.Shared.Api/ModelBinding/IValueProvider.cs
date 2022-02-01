namespace SkillTrail.Shared.Api.ModelBinding;

public interface IValueProvider
{
    ValueProviderResult GetValue(string key);
}