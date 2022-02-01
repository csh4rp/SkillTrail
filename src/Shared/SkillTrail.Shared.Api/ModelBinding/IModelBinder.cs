namespace SkillTrail.Shared.Api.ModelBinding;

public interface IModelBinder
{
    ModelBindingResult Bind(ModelBindingContext context);
}