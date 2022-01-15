using SkillTrail.Shared.Api.ModelBinding;

namespace SkillTrail.Tests.Shared.Api.ModelBinding.Binders;

internal sealed class FakeModelBinder : IModelBinder
{
    public ModelBindingResult Bind(ModelBindingContext bindingContext)
        => ModelBindingResult.Successful(new object());
}