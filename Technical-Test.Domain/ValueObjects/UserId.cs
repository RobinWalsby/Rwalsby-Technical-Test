using Technical_Test.Domain.Base;

namespace Technical_Test.Domain.ValueObjects
{
    public class UserId : ValueObject
    {
        public Guid Value { get; }

        public static UserId Empty => new(Guid.Empty);

        public bool HasValue { get => Value != Guid.Empty; }

        private UserId(Guid value)
        {
            Value = value;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static UserId Create(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("User Id must be a valid Guid", nameof(Create));
            }

            return new UserId(value);
        }
    }
}
