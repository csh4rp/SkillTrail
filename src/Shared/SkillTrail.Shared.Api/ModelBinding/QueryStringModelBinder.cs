using System.Collections;
using BindingFlags = System.Reflection.BindingFlags;

namespace SkillTrail.Shared.Api.ModelBinding;

internal sealed class QueryStringModelBinder : IModelBinder
{
    public ModelBindingResult Bind(ModelBindingContext bindingContext)
    {
        if (string.IsNullOrEmpty(bindingContext.HttpRequestData.Url.Query))
        {
            return ModelBindingResult.Unsuccessful();
        }
        
        var query = bindingContext.HttpRequestData.Url.Query.Substring(1);
        var queryParameters = new Dictionary<string, List<string>>();
        var queryValues = query.Split('&');
        foreach (var queryValue in queryValues)
        {
            var keyValue = queryValue.Split('=');
            var key = keyValue[0].ToLower();
            var value = keyValue[1];
            if (!queryParameters.TryGetValue(key, out var valueList))
            {
                queryParameters.Add(key, new List<string> { value });
            }
            else
            {
                valueList.Add(value);
            }
        }

        if (TryBindUsingDefaultCtor(bindingContext.ModelType, queryParameters, out var model))
        {
            return ModelBindingResult.Successful(model!);
        }

        if (TryBindUsingCtorWithParameters(bindingContext.ModelType, queryParameters, out model))
        {
            return ModelBindingResult.Successful(model!);
        }
        
        return ModelBindingResult.Unsuccessful();
    }

    private static bool TryBindUsingDefaultCtor(Type modelType, Dictionary<string, List<string>> parameters, out object? model)
    {
        var defaultConstructor = modelType.GetConstructor(Type.EmptyTypes);
        if (defaultConstructor is null)
        {
            model = default;
            return false;
        }


        model = defaultConstructor.Invoke(null);
        var properties = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        foreach (var propertyInfo in properties)
        {
            if (!propertyInfo.CanWrite)
            {
                continue;
            }

            var normalizedPropertyName = propertyInfo.Name.ToLower();
            var valueExists = parameters.TryGetValue(normalizedPropertyName, out var propertyValues);
            if (propertyInfo.PropertyType.IsAssignableTo(typeof(IEnumerable)))
            {
                var genericArg = propertyInfo.PropertyType.GetGenericArguments()[0];
                if (valueExists)
                {
                    var convertedCollectionValues = propertyValues!.Select(v => Convert.ChangeType(v, genericArg));
                    var castMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.Cast));
                    var par = castMethod!.MakeGenericMethod(genericArg)
                        .Invoke(null, new object?[] { convertedCollectionValues });

                    propertyInfo.SetValue(model, par);
                }
                else
                {
                    var genericListType = typeof(List<>).MakeGenericType(genericArg);
                    propertyInfo.SetValue(model, Activator.CreateInstance(genericListType));
                }
            }
            else if (valueExists)
            {
                var propertyValue = Convert.ChangeType(propertyValues!.FirstOrDefault(), propertyInfo.PropertyType);
                propertyInfo.SetValue(model, propertyValue);
            }
        }

        return true;
    }

    private static bool TryBindUsingCtorWithParameters(Type modelType, Dictionary<string, List<string>> parameters, out object? model)
    {
        var constructors = modelType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
        if (!constructors.Any())
        {
            model = default;
            return false;
        }

        var ctor = constructors[0];
        var ctorParameters = ctor.GetParameters();
        var invocationArguments = new object?[ctorParameters.Length];
        for (var i = 0; i < ctorParameters.Length; i++)
        {
            var ctorParameter = ctorParameters[i];
            var normalizedPropertyName = ctorParameter.Name!.ToLower();
            var valueExists = parameters.TryGetValue(normalizedPropertyName, out var propertyValues);
            if (ctorParameter.ParameterType.IsAssignableTo(typeof(IEnumerable)))
            {
                var genericArg = ctorParameter.ParameterType.GetGenericArguments()[0];
                if (valueExists)
                {
                    var convertedCollectionValues = propertyValues!.Select(v => Convert.ChangeType(v, genericArg));
                        
                    var castMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.Cast));
                    var par = castMethod!.MakeGenericMethod(genericArg)
                        .Invoke(null, new object?[] { convertedCollectionValues });

                    invocationArguments[i] = par;
                }
                else
                {
                    var genericListType = typeof(List<>).MakeGenericType(genericArg);
                    invocationArguments[i] = Activator.CreateInstance(genericListType);
                }
            }
            else if (valueExists)
            {
                var ctorArgument =
                    Convert.ChangeType(propertyValues!.FirstOrDefault(), ctorParameter.ParameterType);
                invocationArguments[i] = ctorArgument;
            }
            else
            {
                invocationArguments[i] = Activator.CreateInstance(ctorParameters[i].ParameterType);
            }
        }

        model = ctor.Invoke(invocationArguments);
        return true;
    }
}