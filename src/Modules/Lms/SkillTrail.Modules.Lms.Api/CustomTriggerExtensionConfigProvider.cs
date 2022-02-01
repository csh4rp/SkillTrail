using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Host.Listeners;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Azure.WebJobs.Host.Triggers;

namespace SkillTrail.Modules.Lms.Api;


[Extension("Custom")]
public class CustomTriggerExtensionConfigProvider : IExtensionConfigProvider
{
    public void Initialize(ExtensionConfigContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var rule = context.AddBindingRule<CustomTriggerAttribute>();
        rule.BindToTrigger<TProvider>();
    }
}

class TProvider : ITriggerBindingProvider
{
    public async Task<ITriggerBinding> TryCreateAsync(TriggerBindingProviderContext context)
    {
        return new TriggerBinding();
    }
}

class TriggerBinding : ITriggerBinding
{
    public Task<ITriggerData> BindAsync(object value, ValueBindingContext context)
    {
        throw new NotImplementedException();
    }

    public Task<IListener> CreateListenerAsync(ListenerFactoryContext context)
    {
        throw new NotImplementedException();
    }

    public ParameterDescriptor ToParameterDescriptor()
    {
        throw new NotImplementedException();
    }

    public Type TriggerValueType { get; }
    public IReadOnlyDictionary<string, Type> BindingDataContract { get; }
}