using System;
using System.Collections.Generic;
using SkillTrail.Shared.Api.ModelBinding;

namespace SkillTrail.Tests.Shared.Api.Fakes;

internal class FakeModelBinderRegistry : IModelBinderRegistry
{
    private readonly Dictionary<Type, IModelBinder> _binders = new();

    public bool TryGetBinder(Type modelType, out IModelBinder? binder)
        => _binders.TryGetValue(modelType, out binder);

    public void AddBinder(Type modelType, IModelBinder binder)
    {
        _binders.Add(modelType, binder);
    }
}