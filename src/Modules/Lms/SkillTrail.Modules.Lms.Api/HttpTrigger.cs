using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace SkillTrail.Modules.Lms.Api;

public static class HttpTrigger
{
    [Function("HttpTrigger")]
    public static void Run([CustomTriggerAttribute] object req,
        FunctionContext executionContext)
    {

        
    }
}