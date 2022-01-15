using System;
using System.Linq;
using Microsoft.Azure.Functions.Worker.Http;
using NSubstitute;
using Shouldly;
using SkillTrail.Shared.Api.ModelBinding;
using SkillTrail.Tests.Shared.Api.Fakes;
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
        var requestData = new FakeHttpRequestData(new Uri("https://www.api.skilltrail.com/invoke?courseId=1&moduleId=2&userIds=3&userIds=4"), "GET");

        var (wasBound, model) = Act<DefaultCtorModel>(requestData);

        wasBound.ShouldBeTrue();
        model.ShouldNotBeNull();
        model.CourseId.ShouldBe(1);
        model.ModuleId.ShouldBe(2);
        model.UserIds.ShouldNotBeNull();
        model.UserIds.ShouldBe(new[]{3, 4});
    }
    
    [Fact]
    public void Should_BindType_When_TypeHasDefaultCtorAndMissingArguments()
    {
        var requestData = new FakeHttpRequestData(new Uri("https://www.api.skilltrail.com/invoke?courseId=1"), "GET");

        var (wasBound, model) = Act<DefaultCtorModel>(requestData);
        
        wasBound.ShouldBeTrue();
        model.ShouldNotBeNull();
        model.CourseId.ShouldBe(1);
        model.ModuleId.ShouldBe(0);
        model.UserIds.ShouldNotBeNull();
        model.UserIds.ShouldBe(Enumerable.Empty<int>());
    }
    
    [Fact]
    public void Should_BindType_When_TypeHasCustomCtorAndAllArguments()
    {
        var requestData = new FakeHttpRequestData(new Uri("https://www.api.skilltrail.com/invoke?courseId=1&moduleId=2&userIds=3&userIds=4"), "GET");

        var (wasBound, model) = Act<CustomCtorModel>(requestData);
        
        wasBound.ShouldBeTrue();
        model.ShouldNotBeNull();
        model.CourseId.ShouldBe(1);
        model.ModuleId.ShouldBe(2);
        model.UserIds.ShouldNotBeNull();
        model.UserIds.ShouldBe(new[]{3, 4});
    }
    
    [Fact]
    public void Should_BindType_When_TypeHasCustomCtorAndMissingArguments()
    {
        var requestData = new FakeHttpRequestData(new Uri("https://www.api.skilltrail.com/invoke?courseId=1"), "GET");

        var (wasBound, model) = Act<CustomCtorModel>(requestData);

        wasBound.ShouldBeTrue();
        model.ShouldNotBeNull();
        model.CourseId.ShouldBe(1);
        model.ModuleId.ShouldBe(0);
        model.UserIds.ShouldNotBeNull();
        model.UserIds.ShouldBe(Enumerable.Empty<int>());
    }
    
    [Fact]
    public void Should_NotBindType_When_TypeHasCustomPrivateCtor()
    {
        var requestData = new FakeHttpRequestData(new Uri("https://www.api.skilltrail.com/invoke?courseId=1"), "GET");
        
        var (wasBound, model) = Act<PrivateCtorModel>(requestData);

        wasBound.ShouldBeFalse();
        model.ShouldBeNull();
    }

    private (bool wasBound, T? model) Act<T>(HttpRequestData requestData)
    {
        var wasBound = _modelBinder.TryBind<T>(requestData, out var model);
        return (wasBound, model);
    }
}