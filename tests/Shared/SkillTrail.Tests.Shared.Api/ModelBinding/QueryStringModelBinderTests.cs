using System;
using System.Linq;
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
        => _modelBinder = new();
    
    [Fact]
    public void Should_BindType_When_TypeHasDefaultCtorAndAllArguments()
    {
        var requestData = new FakeHttpRequestData(new Uri("https://www.api.skilltrail.com/invoke?courseId=1&moduleId=2&userIds=3&userIds=4"), "GET");
        var bindingContext = new ModelBindingContext(requestData, typeof(DefaultCtorModel), null);

        var result = Act(bindingContext);

        var model = result.Model as DefaultCtorModel;
        result.IsSuccessful.ShouldBeTrue();
        result.Model.ShouldNotBeNull();
        model.ShouldNotBeNull();
        model.CourseId.ShouldBe(1);
        model.ModuleId.ShouldBe(2);
        model.UserIds.ShouldNotBeNull();
        model.UserIds.ShouldBe(new []{3, 4});
    }
    
    [Fact]
    public void Should_BindType_When_TypeHasDefaultCtorAndMissingArguments()
    {
        var requestData = new FakeHttpRequestData(new Uri("https://www.api.skilltrail.com/invoke?courseId=1"), "GET");
        var bindingContext = new ModelBindingContext(requestData, typeof(DefaultCtorModel), null);

        var result = Act(bindingContext);
        
        var model = result.Model as DefaultCtorModel;
        result.IsSuccessful.ShouldBeTrue();
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
        var bindingContext = new ModelBindingContext(requestData, typeof(CustomCtorModel), null);

        var result = Act(bindingContext);
        
        var model = result.Model as CustomCtorModel;
        result.IsSuccessful.ShouldBeTrue();
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
        var bindingContext = new ModelBindingContext(requestData, typeof(CustomCtorModel), null);

        var result = Act(bindingContext);

        var model = result.Model as CustomCtorModel;
        result.IsSuccessful.ShouldBeTrue();
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
        var bindingContext = new ModelBindingContext(requestData, typeof(PrivateCtorModel), null);
        
        var result = Act(bindingContext);

        result.IsSuccessful.ShouldBeFalse();
        result.Model.ShouldBeNull();
    }

    private ModelBindingResult Act(ModelBindingContext bindingContext)
        => _modelBinder.Bind(bindingContext);
    
}