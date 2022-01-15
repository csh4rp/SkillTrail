namespace SkillTrail.Shared.Api.ModelBinding;

internal sealed class ModelBinderRegistry
{
    private readonly Dictionary<Type, IModelBinder> _binders = new();

    public bool TryGetBinder(Type type, out IModelBinder? binder)
        => _binders.TryGetValue(type, out binder);
    
}