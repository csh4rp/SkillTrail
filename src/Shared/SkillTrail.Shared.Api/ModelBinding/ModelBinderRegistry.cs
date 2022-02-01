namespace SkillTrail.Shared.Api.ModelBinding;

internal sealed class ModelBinderRegistry : IModelBinderRegistry
{
    private readonly Dictionary<Type, IModelBinder> _modelBinders;

    public ModelBinderRegistry(Dictionary<Type, IModelBinder> modelBinders)
        => _modelBinders = modelBinders;
    
    public bool TryGet(Type modelType, out IModelBinder? modelBinder)
        => _modelBinders.TryGetValue(modelType, out modelBinder);
}