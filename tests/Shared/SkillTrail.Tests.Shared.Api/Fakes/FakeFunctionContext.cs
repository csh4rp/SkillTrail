using System;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker;

namespace SkillTrail.Tests.Shared.Api.Fakes;

public class FakeFunctionContext : FunctionContext
{
    public override string InvocationId => null!;
    public override string FunctionId => null!;
    public override TraceContext TraceContext => null!;
    public override BindingContext BindingContext => null!;
    public override RetryContext RetryContext => null!;
    public override IServiceProvider InstanceServices { get; set; } = null!;
    public override FunctionDefinition FunctionDefinition => null!;
    public override IDictionary<object, object> Items { get; set; } = null!;
    public override IInvocationFeatures Features => null!;
}