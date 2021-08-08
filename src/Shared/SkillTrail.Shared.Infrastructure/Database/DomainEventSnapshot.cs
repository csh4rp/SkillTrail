using System;
using System.Text.Json;

namespace SkillTrail.Shared.Infrastructure.Database
{
    public class DomainEventSnapshot
    {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public JsonDocument AggregateData { get; set; }
        public JsonDocument EventData { get; set; }
    }
}