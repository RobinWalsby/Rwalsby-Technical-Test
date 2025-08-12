using Technical_Test.Common.Helpers;
using Technical_Test.Domain.Base;

namespace Technical_Test.Domain.ValueObjects
{
    public class TimesheetHours : ValueObject
    {
        public decimal? Value { get; } = null;

        public static TimesheetHours Empty => new TimesheetHours();

        private TimesheetHours()
        {
        }

        private TimesheetHours(decimal value)
        {
            Value = value;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        private static bool IsValidFractional(decimal value)
        {
            var fractionalPart = Math.Abs(MathHelper.Fractional(value));
            if (fractionalPart == 0 || fractionalPart == 0.25m || fractionalPart == 0.5m)
            {
                return true;
            }

            return false;
        }

        public static TimesheetHours Create(decimal value)
        {
            if (value > 24)
            {
                throw new ArgumentException("Invalid hours for a day cannot not exceed 24", nameof(value));
            }
            if (!IsValidFractional(value))
            {
                throw new ArgumentException("Invalid hours fractional is invalid", nameof(value));
            }

            return new TimesheetHours(value);
        }

        public static TimesheetHours Create(string value)
        {
            decimal hoursValue;

            if (string.IsNullOrEmpty(value))
            {
                return TimesheetHours.Empty;
            }
            if (!decimal.TryParse(value, out hoursValue))
            {
                throw new ArgumentException("TimesheetDate must be a valid hours value and possibly fractional minutes (h.5 or h.25)", nameof(value));
            }

            return Create(hoursValue);
        }
    }
}
