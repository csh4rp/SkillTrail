using System.Collections;
using BindingFlags = System.Reflection.BindingFlags;

namespace SkillTrail.Shared.Api.ModelBinding;

public class QueryStringModelBinder
{
    public T? Bind<T>(Uri uri)
    {
        var query = uri.Query.Substring(1);
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
        
        if (TryBindUsingDefaultCtor(queryParameters, out T? instance) || TryBindUsingCtorWithParameters(queryParameters, out instance))
        {
            return instance;
        }

        return default;
    }

    private static bool TryBindUsingDefaultCtor<T>(Dictionary<string, List<string>> parameters, out T? instance)
    {
        var defaultConstructor = typeof(T).GetConstructor(Type.EmptyTypes);
        if (defaultConstructor is null)
        {
            instance = default;
            return false;
        }


        instance = (T)defaultConstructor.Invoke(null);
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
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

                    propertyInfo.SetValue(instance, par);
                }
                else
                {
                    var genericListType = typeof(List<>).MakeGenericType(genericArg);
                    propertyInfo.SetValue(instance, Activator.CreateInstance(genericListType));
                }
            }
            else if (valueExists)
            {
                var propertyValue = Convert.ChangeType(propertyValues!.FirstOrDefault(), propertyInfo.PropertyType);
                propertyInfo.SetValue(instance, propertyValue);
            }
        }

        return true;
    }

    private static bool TryBindUsingCtorWithParameters<T>(Dictionary<string, List<string>> parameters, out T? instance)
    {
        var constructors = typeof(T).GetConstructors(BindingFlags.Public | BindingFlags.Instance);
        if (!constructors.Any())
        {
            instance = default;
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

        instance = (T)ctor.Invoke(invocationArguments);
        return true;
    }
}