using Technical_Test.Domain.Base;

namespace Technical_Test.Domain.ValueObjects
{
    public class UserName : ValueObject
    {
        public string Value { get; }

        public static UserName Empty => new (string.Empty);

        private UserName(string value)
        {
            Value = value;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public static UserName Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("User name must not be empty", nameof(value));
            }
            if (value.Length > 50)
            {
                throw new ArgumentException("User name cannot exceed more than 50 characters", nameof(value));
            }

            return new UserName(value);
        }
    }
}
