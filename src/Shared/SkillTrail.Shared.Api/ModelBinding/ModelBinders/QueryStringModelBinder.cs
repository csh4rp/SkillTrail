using System.Reflection;

namespace SkillTrail.Shared.Api.ModelBinding.ModelBinders;

internal sealed class QueryStringModelBinder : IModelBinder
{
    public ModelBindingResult Bind(ModelBindingContext context)
    {
        var ctor = GetConstructor(context.ModelType);
        if (ctor is null)
        {
            return ModelBindingResult.Unsuccessful();
        }
        
        var ctorParameters = ctor.GetParameters();

        if (ctorParameters.Length == 0)
        {
            var obj = CreateByDefaultCtor(ctor, context.ValueProvider);
            return ModelBindingResult.Successful(obj);
        }
        else
        {
            var obj = CreateByCustomCtor(ctor, ctorParameters, context.ValueProvider);
            return ModelBindingResult.Successful(obj);
        }
    }

    private static ConstructorInfo? GetConstructor(Type type)
    {
        var constructors = type.GetConstructors();
        var defaultCtor = constructors.FirstOrDefault(c => c.GetParameters().Length == 0);
        if (defaultCtor is not null)
        {
            return defaultCtor;
        }

        return constructors.Length > 1 ? null : constructors[0];
    }

    private static object CreateByDefaultCtor(ConstructorInfo ctor, IValueProvider valueProvider)
    {
        var obj = ctor.Invoke(Array.Empty<object?>());
        var properties = ctor.DeclaringType!.GetProperties()
            .Where(p => p.CanWrite);
            
        foreach (var propertyInfo in properties)
        {
            var valueProviderResult = valueProvider.GetValue(propertyInfo.Name);
            if (valueProviderResult.IsSuccessful)
            {
                propertyInfo.SetValue(obj, valueProviderResult.Value);
            }
        }

        return obj;
    }
    
    private static object CreateByCustomCtor(ConstructorInfo ctor, ParameterInfo[] ctorParameters, IValueProvider valueProvider)
    {
        var parameterValues = new object?[ctorParameters.Length];
        for (var i = 0; i < ctorParameters.Length; i++)
        {
            var ctorParameter = ctorParameters[i];
            var valueProviderResult = valueProvider.GetValue(ctorParameter.Name!);
            if (valueProviderResult.IsSuccessful)
            {
                parameterValues[i] = valueProviderResult.Value;
            }
            else
            {
                parameterValues[i] = null;
            }
        }

        return ctor.Invoke(parameterValues);
    }
}