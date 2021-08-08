using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using SkillTrail.Shared.Infrastructure.Identity;

namespace SkillTrail.Shared.Infrastructure.Internal
{
    internal sealed class ApplicationContext : IApplicationContext
    {
        public ApplicationContext(IHttpContextAccessor httpContextAccessor)
        {
            TenantId = GetTenantId(httpContextAccessor);
            CorrelationId = GetCorrelationId(httpContextAccessor);
        }

        public Guid TenantId { get; private set; }
        public Guid CorrelationId { get; }

        private static Guid GetTenantId(IHttpContextAccessor httpContextAccessor)
        {
            var claim =  httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(CustomClaimTypes.TenantId));
            return claim is not null ? Guid.Parse(claim.Value) : Guid.Empty;
        }

        private static Guid GetCorrelationId(IHttpContextAccessor httpContextAccessor) =>
            httpContextAccessor.HttpContext.Items.ContainsKey("RequestId")
                ? (Guid) httpContextAccessor.HttpContext.Items["RequestId"]
                : Guid.NewGuid();
    }
}