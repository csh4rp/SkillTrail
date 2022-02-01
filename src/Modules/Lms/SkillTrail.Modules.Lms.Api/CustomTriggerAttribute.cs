using Microsoft.Azure.Functions.Worker.Extensions.Abstractions;

[assembly: ExtensionInformation("SkillTrail.Modules.Lms.Api", "2.0.0")]

namespace SkillTrail.Modules.Lms.Api;

public class CustomTriggerAttribute : Microsoft.Azure.Functions.Worker.Extensions.Abstractions.TriggerBindingAttribute
{
    
}