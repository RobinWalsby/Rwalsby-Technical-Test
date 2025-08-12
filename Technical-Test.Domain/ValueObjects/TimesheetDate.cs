using Technical_Test.Domain.Base;

namespace Technical_Test.Domain.ValueObjects
{
    public class TimesheetDate : ValueObject
    {
        public DateOnly Value { get; }

        public DateTime ValueDateTime { get => Value.ToDateTime(TimeOnly.Parse("10:00 PM")); }

        public static TimesheetDate Empty => new(DateOnly.MinValue);

        private TimesheetDate(DateOnly value)
        {
            Value = value;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static TimesheetDate Create(DateOnly value)
        {
            if (value == DateOnly.MinValue || value == DateOnly.MaxValue)
            {
                throw new ArgumentException("TimesheetDate must be a valid date", nameof(value));
            }
            if (value.Year >= DateTime.Now.Date.AddYears(2).Year)
            {
                throw new ArgumentException("TimesheetDate must be a valid future date", nameof(value));
            }
            if (value.Year <= DateTime.Now.Date.AddYears(-2).Year)
            {
                throw new ArgumentException("TimesheetDate must be a valid past date", nameof(value));
            }

            return new TimesheetDate(value);
        }

        public static TimesheetDate Create(string value)
        {
            DateOnly dateValue;

            if (!DateOnly.TryParse(value, out dateValue))
            {
                throw new ArgumentException("TimesheetDate must be a valid date", nameof(value));
            }

            return Create(dateValue);
        }

        public static TimesheetDate Create(DateTime value)
        {
            return Create(DateOnly.FromDateTime(value));
        }
    }
}
