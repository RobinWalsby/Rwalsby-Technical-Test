using Technical_Test.Domain.Base;

namespace Technical_Test.Domain.ValueObjects
{
    public class TimesheetId : ValueObject
    {
        public Guid Value { get; }

        public static TimesheetId Empty => new(Guid.Empty);

        public bool HasValue { get => Value != Guid.Empty; }

        private TimesheetId(Guid value)
        {
            Value = value;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static TimesheetId Create(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Timesheet Id must be a valid Guid", nameof(Create));
            }

            return new TimesheetId(value);
        }
    }
}
