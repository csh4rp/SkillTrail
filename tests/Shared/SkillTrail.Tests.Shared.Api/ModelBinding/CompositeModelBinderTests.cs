using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using SkillTrail.Shared.Api.ModelBinding;
using SkillTrail.Tests.Shared.Api.Fakes;
using SkillTrail.Tests.Shared.Api.ModelBinding.Models;
using Xunit;

namespace SkillTrail.Tests.Shared.Api.ModelBinding;

public class CompositeModelBinderTests
{
    private CompositeModelBinder _modelBinder;

    [Fact]
    public void Should()
    {
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();

        _modelBinder = new CompositeModelBinder(provider);
        var requestData = new FakeHttpRequestData(new Uri("https://www.skilltrail.com"), "POST");
        requestData.SetBody(JsonStream(), "application/json");

        var wasBound = _modelBinder.TryBind<DefaultCtorModel>(requestData, out var model);
        
        

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
}