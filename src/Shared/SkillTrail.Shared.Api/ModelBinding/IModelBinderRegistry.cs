namespace SkillTrail.Shared.Api.ModelBinding;

public interface IModelBinderRegistry
{
    bool TryGetBinder(Type modelType, out IModelBinder? binder);
}