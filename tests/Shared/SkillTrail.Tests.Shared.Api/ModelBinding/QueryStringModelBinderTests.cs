using System;
using System.Linq;
using SkillTrail.Shared.Api.ModelBinding;
using SkillTrail.Tests.Shared.Api.ModelBinding.Models;
using Xunit;

namespace SkillTrail.Tests.Shared.Api.ModelBinding;

public class QueryStringModelBinderTests
{
    private readonly QueryStringModelBinder _modelBinder;

    public QueryStringModelBinderTests()
    {
        _modelBinder = new();
    }

    [Fact]
    public void Should_BindType_When_TypeHasDefaultCtorAndAllArguments()
    {
        var uri = new Uri("https://www.api.skilltrail.com/invoke?courseId=1&moduleId=2&userIds=3&userIds=4");
        
        var result = Act<DefaultCtorModel>(uri);

        Assert.NotNull(result);
        Assert.Equal(1, result!.CourseId);
        Assert.Equal(2, result.ModuleId);
        Assert.NotNull(result.UserIds);
        Assert.Equal(new[]{3, 4}, result.UserIds);
    }
    
    [Fact]
    public void Should_BindType_When_TypeHasDefaultCtorAndMissingArguments()
    {
        var uri = new Uri("https://www.api.skilltrail.com/invoke?courseId=1");
        
        var result = Act<DefaultCtorModel>(uri);

        Assert.NotNull(result);
        Assert.Equal(1, result!.CourseId);
        Assert.Equal(0, result.ModuleId);
        Assert.NotNull(result.UserIds);
        Assert.Equal(Enumerable.Empty<int>(), result.UserIds);
    }
    
    [Fact]
    public void Should_BindType_When_TypeHasCustomCtorAndAllArguments()
    {
        var uri = new Uri("https://www.api.skilltrail.com/invoke?courseId=1&moduleId=2&userIds=3&userIds=4");

        var result = Act<CustomCtorModel>(uri);

        Assert.NotNull(result);
        Assert.Equal(1, result!.CourseId);
        Assert.Equal(2, result.ModuleId);
        Assert.NotNull(result.UserIds);
        Assert.Equal(new[]{3, 4}, result.UserIds);
    }
    
    [Fact]
    public void Should_BindType_When_TypeHasCustomCtorAndMissingArguments()
    {
        var uri = new Uri("https://www.api.skilltrail.com/invoke?courseId=1");
        
        var result = Act<DefaultCtorModel>(uri);

        Assert.NotNull(result);
        Assert.Equal(1, result!.CourseId);
        Assert.Equal(0, result.ModuleId);
        Assert.NotNull(result.UserIds);
        Assert.Equal(Enumerable.Empty<int>(), result.UserIds);
    }
    
    [Fact]
    public void Should_NotBindType_When_TypeHasCustomPrivateCtor()
    {
        var uri = new Uri("https://www.api.skilltrail.com/invoke?courseId=1");
        
        var result = Act<PrivateCtorModel>(uri);

        Assert.Null(result);
    }

    private T? Act<T>(Uri uri) => _modelBinder.Bind<T>(uri);
}