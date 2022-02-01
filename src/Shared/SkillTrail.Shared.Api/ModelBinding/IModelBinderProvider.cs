namespace SkillTrail.Shared.Api.ModelBinding;

public interface IModelBinderProvider
{
    IModelBinder? GetBinder(ModelBindingContext modelBindingContext);
}