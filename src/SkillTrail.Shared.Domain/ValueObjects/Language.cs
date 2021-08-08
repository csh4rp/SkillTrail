using System.Globalization;
using SkillTrail.Shared.Domain.Abstract;

namespace SkillTrail.Shared.Domain.ValueObjects
{
    public record Language : IValueObject
    {
        private Language()
        {
        }
        
        public Language(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public CultureInfo GetCultureInfo() => CultureInfo.GetCultureInfo(Value);

        public override string ToString() => Value;

        public static Language FromCulture(CultureInfo cultureInfo) => new(cultureInfo.Name);
    }
}