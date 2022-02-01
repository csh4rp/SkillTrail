using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using SkillTrail.Modules.Lms.Api;

[assembly: WebJobsStartup(typeof(CustomStartup))]

namespace SkillTrail.Modules.Lms.Api;

public class CustomStartup : IWebJobsStartup
{
    public void Configure(IWebJobsBuilder builder)
    {
        builder.AddExtension<CustomTriggerExtensionConfigProvider>();
    }
}