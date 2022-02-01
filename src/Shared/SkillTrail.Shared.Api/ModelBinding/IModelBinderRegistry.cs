namespace SkillTrail.Shared.Api.ModelBinding;

public interface IModelBinderRegistry
{
    bool TryGet(Type modelType, out IModelBinder? modelBinder);
}