using System;

namespace SkillTrail.Shared.Infrastructure
{
    public interface IApplicationContext
    {
        Guid TenantId { get; }
        Guid CorrelationId { get; }
    }
}