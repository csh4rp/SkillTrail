using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using Microsoft.Azure.Functions.Worker.Http;
using Shouldly;
using SkillTrail.Shared.Api.ModelBinding;
using SkillTrail.Tests.Shared.Api.Fakes;
using SkillTrail.Tests.Shared.Api.ModelBinding.Models;
using Xunit;

namespace SkillTrail.Tests.Shared.Api.ModelBinding;

public class JsonModelBinderTests
{
    private readonly JsonModelBinder _modelBinder;

    public JsonModelBinderTests()
    {
        _modelBinder = new JsonModelBinder();
    }

    [Fact]
    public void Should_BindModel_When_BodyContainsJson()
    {
        var requestData = new FakeHttpRequestData(new Uri("https://skilltrail.com"), "POST");
        requestData.SetBody(JsonStream(), "application/json");

        var (wasBound, model) = Act<DefaultCtorModel>(requestData);
        
        wasBound.ShouldBeTrue();
        model.ShouldNotBeNull();
        model.CourseId.ShouldBe(1);
        model.ModuleId.ShouldBe(2);
        model.UserIds.ShouldNotBeNull();
        model.UserIds.ShouldBe(new []{3,4});
    }
    
    [Fact]
    public void Should_NotBindModel_When_JsonHeaderIsNotSet()
    {
        var requestData = new FakeHttpRequestData(new Uri("https://skilltrail.com"), "POST");
        requestData.SetBody(XmlStream(), "application/xml");

        var (wasBound, model) = Act<DefaultCtorModel>(requestData);
        
        wasBound.ShouldBeFalse();
        model.ShouldBeNull();
    }
    
    [Fact]
    public void Should_NotBindModel_When_BodyIsEmpty()
    {
        var requestData = new FakeHttpRequestData(new Uri("https://skilltrail.com"), "POST");
        requestData.SetBody(EmptyStream(), "application/json");

        var (wasBound, model) = Act<DefaultCtorModel>(requestData);
        
        wasBound.ShouldBeFalse();
        model.ShouldBeNull();
    }

    private (bool wasBound, T? model) Act<T>(HttpRequestData requestData)
    {
        var wasBound = _modelBinder.TryBind<T>(requestData, out var model);
        return (wasBound, model);
    }

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

    private static Stream XmlStream()
    {
        var stream = new MemoryStream();
        var serializer = new XmlSerializer(typeof(int));
        serializer.Serialize(stream, 1);
        return stream;
    }

    private static Stream EmptyStream() => new MemoryStream();
}