using Technical_Test.Domain.Base;

namespace Technical_Test.Domain.ValueObjects
{
    public class TimesheetDescription : ValueObject
    {
        public string Value { get; }

        public static TimesheetDescription Empty => new (string.Empty);

        private TimesheetDescription(string value)
        {
            Value = value;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static TimesheetDescription Create(string value)
        {
            if (value.Length > 200)
            {
                throw new ArgumentException("User name cannot exceed more than 200 characters", nameof(value));
            }

            return new TimesheetDescription(value);
        }
    }
}
