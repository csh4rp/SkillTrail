using System;

namespace SkillTrail.Shared.Domain.Utils
{
    public sealed class DateTimeProvider : IDateTimeProvider
    {
        private DateTimeProvider()
        {
        }
        
        public static IDateTimeProvider Instance { get; set; } = new DateTimeProvider();

        public DateTime CurrentDateTime => DateTime.UtcNow;

        public static DateTime Now => Instance.CurrentDateTime;
    }
}