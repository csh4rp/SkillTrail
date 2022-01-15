using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Shouldly;
using SkillTrail.Shared.Api.ModelBinding;
using SkillTrail.Tests.Shared.Api.Fakes;
using SkillTrail.Tests.Shared.Api.ModelBinding.Binders;
using SkillTrail.Tests.Shared.Api.ModelBinding.Models;
using Xunit;

namespace SkillTrail.Tests.Shared.Api.ModelBinding;

public class CompositeModelBinderTests
{
    private readonly CompositeModelBinder _modelBinder;
    private readonly FakeModelBinderRegistry _modelBinderRegistry;

    public CompositeModelBinderTests()
    {
        _modelBinderRegistry = new FakeModelBinderRegistry();
        _modelBinder = new(_modelBinderRegistry);
    }
    
    [Fact]
    public void Should_BindModelFromBody_When_BodyHasModel()
    {
        var requestData = new FakeHttpRequestData(new Uri("https://www.skilltrail.com/invoke"), "POST");
        requestData.SetBody(JsonStream(), "application/json");
        var bindingContext = new ModelBindingContext(requestData, typeof(CustomCtorModel), BindingDataSource.Body);

        var result = Act(bindingContext);

        var model = result.Model as CustomCtorModel;
        result.IsSuccessful.ShouldBeTrue();
        model.ShouldNotBeNull();
        model.CourseId.ShouldBe(1);
        model.ModuleId.ShouldBe(2);
        model.UserIds.ShouldNotBeNull();
        model.UserIds.ShouldBe(new []{3, 4});
    }
    
    [Fact]
    public void Should_BindModelFromQuery_When_QueryHasModel()
    {
        var requestData = new FakeHttpRequestData(new Uri("https://www.skilltrail.com/invoke?courseId=1&moduleId=2&userIds=3&userIds=4"), "GET");
        var bindingContext = new ModelBindingContext(requestData, typeof(CustomCtorModel), BindingDataSource.Query);
        
        var result = Act(bindingContext);

        var model = result.Model as CustomCtorModel;
        result.IsSuccessful.ShouldBeTrue();
        model.ShouldNotBeNull();
        model.CourseId.ShouldBe(1);
        model.ModuleId.ShouldBe(2);
        model.UserIds.ShouldNotBeNull();
        model.UserIds.ShouldBe(new []{3, 4});
    }
    
    [Fact]
    public void Should_NotBindModelWhen_ThereAreNoValuesInBodyAndQuery()
    {
        var requestData = new FakeHttpRequestData(new Uri("https://www.skilltrail.com/invoke"), "GET");
        var bindingContext = new ModelBindingContext(requestData, typeof(CustomCtorModel), null);
        
        var result = Act(bindingContext);
        
        result.IsSuccessful.ShouldBeFalse();
        result.Model.ShouldBeNull();
    }

    [Fact]
    public void Should_UseCustomBinder_When_CustomBinderIsRegistered()
    {
        var binder = new FakeModelBinder();
        _modelBinderRegistry.AddBinder(typeof(CustomCtorModel), binder);
        var requestData = new FakeHttpRequestData(new Uri("https://www.skilltrail.com/invoke"), "GET");
        var bindingContext = new ModelBindingContext(requestData, typeof(CustomCtorModel), null);
        
        var result = Act(bindingContext);

        result.IsSuccessful.ShouldBeTrue();
        result.Model.ShouldNotBeNull();
    }

    private ModelBindingResult Act(ModelBindingContext bindingContext)
        => _modelBinder.Bind(bindingContext);
    
    private static Stream JsonStream()
    {
        var model = new DefaultCtorModel
        {
            CourseId = 1,
            ModuleId = 2,
            UserIds = new[] { 3, 4 }
        };
        
        var serialized = JsonSerializer.Serialize(model);
        var bytes = Encoding.UTF8.GetBytes(serialized);
        return new MemoryStream(bytes);
    }
}