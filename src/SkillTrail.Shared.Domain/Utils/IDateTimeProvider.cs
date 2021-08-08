using System;

namespace SkillTrail.Shared.Domain.Utils
{
    public interface IDateTimeProvider
    {
        DateTime CurrentDateTime { get; }
    }
}